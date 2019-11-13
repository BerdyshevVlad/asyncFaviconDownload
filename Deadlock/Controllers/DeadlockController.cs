using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Deadlock.Controllers
{
    public class MyLibrary
    {
        // My "library" method.
        public async Task<JObject> GetJsonAsync(Uri uri)
        {
            // (real-world code shouldn't use HttpClient in a using block; this is just example code)
            using (var client = new HttpClient())
            {
                var jsonString = await client.GetStringAsync(uri);
                return JObject.Parse(jsonString);
            }
        }
    }

    [Route("api/deadlock")]
    [ApiController]
    public class DeadlockController : Controller
    {
        private MyLibrary _myLibrary;

        public DeadlockController()
        {
            _myLibrary = new MyLibrary();
        }

        // My "top-level" method.
        [Route("GetDeadlock")]
        [HttpGet]
        public string GetDeadlock()
        {
            //чтобы воспроизвести deadlock, в дебаге подождать 1-2 мин после получения jsonTask
            var uri = new Uri("https://jsonplaceholder.typicode.com/todos/1");
            var jsonTask = _myLibrary.GetJsonAsync(uri);
            return jsonTask.Result.ToString();
        }

        [Route("Get")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var uri = new Uri("https://jsonplaceholder.typicode.com/todos/1");
            var jsonTask = await _myLibrary.GetJsonAsync(uri);
            return Ok(jsonTask.ToString());
        }

        [Route("GetDeadlock2")]
        [HttpGet]
        public String DownloadStringV3()
        {
            // NOT SAFE, instant deadlock when called from UI thread
            // deadlock when called from threadpool, works fine on console
            var client = new HttpClient();
            var request = client.GetAsync("https://jsonplaceholder.typicode.com/todos/1").Result;
            var download = request.Content.ReadAsStringAsync().Result;
            return download;
        }


        //1) The top-level method calls GetJsonAsync(within the UI/ASP.NET context).
        //2) GetJsonAsync starts the REST request by calling HttpClient.GetStringAsync(still within the context).
        //3) GetStringAsync returns an uncompleted Task, indicating the REST request is not complete.
        //4) GetJsonAsync awaits the Task returned by GetStringAsync. The context is captured and will be used to continue running the GetJsonAsync method later.GetJsonAsync returns an uncompleted Task, indicating that the GetJsonAsync method is not complete.
        //5)The top-level method synchronously blocks on the Task returned by GetJsonAsync. This blocks the context thread.
        //6)… Eventually, the REST request will complete.This completes the Task that was returned by GetStringAsync.
        //7) The continuation for GetJsonAsync is now ready to run, and it waits for the context to be available so it can execute in the context.
        //8) Deadlock.The top-level method is blocking the context thread, waiting for GetJsonAsync to complete, and GetJsonAsync is waiting for the context to be free so it can complete.

        //Синхронный метод GetDeadlock вызвал асинхронный GetJsonAsync и заблокировал поток (.Result блокирует поток), ожидая, что GetJsonAsync вернет данные. А GetJsonAsync, после завершения, не может вернуть данные в поток, т.к. он заблокирован. Deadlock.

        //The problem is that if you blocked the thread which is supposed to complete the I/O, then there won’t be a thread to complete the I/O.

        //https://medium.com/rubrikkgroup/understanding-async-avoiding-deadlocks-e41f8f2c6f5d

        //https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html

        //async js 
        //https://medium.com/@stasonmars/%D0%BF%D0%BE%D0%BB%D0%BD%D0%BE%D0%B5-%D0%BF%D0%BE%D0%BD%D0%B8%D0%BC%D0%B0%D0%BD%D0%B8%D0%B5-%D1%81%D0%B8%D0%BD%D1%85%D1%80%D0%BE%D0%BD%D0%BD%D0%BE%D0%B3%D0%BE-%D0%B8-%D0%B0%D1%81%D0%B8%D0%BD%D1%85%D1%80%D0%BE%D0%BD%D0%BD%D0%BE%D0%B3%D0%BE-javascript-%D1%81-async-await-ba5f47f4436
    }
}