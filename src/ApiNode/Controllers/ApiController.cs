using System;
using System.Threading.Tasks;
using Akka.Actor;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Actors;

namespace ApiNode.Controllers
{
    public class ApiController : ControllerBase
    {
        [HttpGet, Route("{name}")]
        public async Task<ActionResult> Get(string name)
        {
            var message = new ShardEnvelope(name, Customer.ShowBasket.Instance);
            var timeout = TimeSpan.FromSeconds(10);

            var basket = await Actors.Instance.CustomerProxy.Ask<Customer.Basket>(message, timeout);

            return Ok(basket);
        }

        [HttpPost, Route("{name}")]
        public ActionResult Post([FromBody] AddItemRequest request, string name)
        {
            var message = new ShardEnvelope(name, new Customer.AddItem(request.Product, request.Quantity));

            Actors.Instance.CustomerProxy.Tell(message, ActorRefs.NoSender);

            return Ok();
        }

        [HttpDelete, Route("{name}")]
        public ActionResult Delete(string name)
        {
            var message = new ShardEnvelope(name, Customer.EmptyBasket.Instance);

            Actors.Instance.CustomerProxy.Tell(message, ActorRefs.NoSender);

            return Ok();
        }

        public class AddItemRequest
        {
            public string Product { get; set; }
            public int Quantity { get; set; }
        }
    }
}
