using System;
using System.Collections.Generic;
using Akka;
using Akka.Actor;
using Akka.Persistence;

namespace Shared.Actors
{
    public class Customer : ReceivePersistentActor
    {
        #region Messages

        public sealed class AddItem
        {
            public readonly string Product;
            public readonly int Quantity;

            public AddItem(string product, int quantity)
            {
                Product = product;
                Quantity = quantity;
            }
        }

        public sealed class CartItem
        {
            public readonly string Product;
            public readonly int Quantity;

            public CartItem(string product, int quantity)
            {
                Product = product;
                Quantity = quantity;
            }
        }

        public sealed class ShowBasket
        {
            public static ShowBasket Instance = new ShowBasket();
            private ShowBasket() { }
        }

        public sealed class Basket
        {
            public readonly string ShardId;
            public readonly string EntityId;
            public readonly Dictionary<string, int> Items;

            public Basket(string shardId, string entityId, Dictionary<string, int> items)
            {
                ShardId = shardId;
                EntityId = entityId;
                Items = items;
            }
        }

        public sealed class EmptyBasket
        {
            public static EmptyBasket Instance = new EmptyBasket();
            private EmptyBasket() { }
        }
        #endregion

        public const string TypeName = "customer";

        public override string PersistenceId { get; } = $"{ShardId}/{EntityId}";

        private static string ShardId => Context.Parent.Path.Name;
        private static string EntityId => Context.Self.Path.Name;

        private static readonly TimeSpan InactivityWindow = TimeSpan.FromSeconds(20);

        public ICollection<CartItem> CartItems = new List<CartItem>();

        public Customer()
        {
            Recover<CartItem>(purchased => CartItems.Add(purchased));

            CommandAny(Handle);
        }

        protected override void PreStart()
        {
            Print.Message($"Waking up {EntityId} on shard {ShardId}.", ConsoleColor.Green);

            SetReceiveTimeout(InactivityWindow);
        }

        public void Handle(object message)
        {
            SetReceiveTimeout(InactivityWindow);

            message
                .Match()
                .With<AddItem>(HandleAddItem)
                .With<ShowBasket>(HandleShowBasket)
                .With<EmptyBasket>(HandleEmptyBasket)
                .With<ReceiveTimeout>(HandleReceiveTimeout)
                .Default(x => Print.Message("Unhandled message type.", ConsoleColor.Red));
        }

        private void HandleAddItem(AddItem purchase)
        {
            Persist(new CartItem(purchase.Product, purchase.Quantity), item =>
            {
                CartItems.Add(item);

                Print.Message($"> {EntityId} added {item.Product} x {item.Quantity}.");
            });
        }

        private void HandleShowBasket()
        {
            var items = new Dictionary<string, int>();
            foreach (var item in CartItems)
            {
                if (items.ContainsKey(item.Product))
                    items[item.Product] += item.Quantity;

                else
                    items.Add(item.Product, item.Quantity);
            }

            Sender.Tell(new Basket(ShardId, EntityId, items), Self);

            Print.Message($"> {EntityId} requested basket contents.");
        }

        private void HandleEmptyBasket()
        {
            DeleteMessages(long.MaxValue);

            CartItems = new List<CartItem>();

            Print.Message($"> {EntityId} emptied their cart.");
        }

        private void HandleReceiveTimeout()
        {
            Print.Message($"{EntityId} is inactive. Shutting down...", ConsoleColor.Green);

            Context.Stop(Self);
        }
    }
}
