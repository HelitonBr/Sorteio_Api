using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sorteio.API
{
    public class Response
    {
        public Response()
        {
            Errors = new List<string>();
        }

        public IList<string> Errors { get; }
        public bool Success => !Errors.Any();

        public void Add(string errorMessage)
        {
            if (errorMessage == null || errorMessage.Trim().Length == 0)
            {
                return;
            }

            Errors.Add(errorMessage);
        }

        public void Add(IList<string> errorsMessages)
        {
            if (errorsMessages.Count <= 0) return;

            foreach (var message in errorsMessages)
            {
                Errors.Add(message);
            }
        }
    }

    public class Response<TResult> : Response
    {
        public TResult Result { get; set; }
    }
}
