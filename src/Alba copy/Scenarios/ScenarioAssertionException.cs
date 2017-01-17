using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if NET46
using System.Runtime.Serialization;
#endif
using Baseline;

namespace Alba.Scenarios
{
#if NET46
    [Serializable]
#endif
    public class ScenarioAssertionException : Exception
    {
        private readonly IList<string> _messages = new List<string>();

        public ScenarioAssertionException()
        {
        }

#if NET46
        protected ScenarioAssertionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
        public void Add(string message)
        {
            _messages.Add(message);
        }

        public void AssertAll()
        {
            if (_messages.Any())
            {
                throw this;
            }
        }

        public IEnumerable<string> Messages => _messages;

        public override string Message
        {
            get
            {
                var writer = new StringWriter();
                _messages.Each(x => writer.WriteLine((string) x));

                if (Body.IsNotEmpty())
                {
                    writer.WriteLine();
                    writer.WriteLine();
                    writer.WriteLine("Actual body text was:");
                    writer.WriteLine();
                    writer.WriteLine(Body);
                }

                return writer.ToString();
            }
        }

        public string Body { get; set; }

        public string ReadBody(Scenario scenario)
        {
            if (Body == null)
            {
                Body = scenario.Request.ResponseStream().ReadAllText();
            }

            return Body;
        }
    }
}