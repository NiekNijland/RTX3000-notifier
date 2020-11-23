using System;
using System.Collections.Generic;
using System.Text;

namespace RTX3000_notifier.Model
{
    public class Subscriber
    {
        public string Id { get; set; }
        public string Email { get; private set; }

        public List<Videocard> Interests { get; set; }

        public Subscriber(string id, string email, List<Videocard> interests)
        {
            this.Id = id;
            this.Email = email;
            this.Interests = interests;
        }
    }
}
