using Microsoft.AspNetCore.Mvc;
using ShopApi.Model.Models;
using ShopApi.Service.Abstractions;
using ShopApi.Service.Services;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Net;
using System.Net.Http;


namespace ShopApi.Web.Infrastructure.core
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        private IServiceManager _serviceManager;
        public ApiControllerBase(IServiceManager serviceManager) 
        {
            this._serviceManager = serviceManager;
        }

        protected HttpResponseMessage CreateHttpResponse(HttpRequestMessage requestMessage, Func<HttpResponseMessage> func)
        {
            HttpResponseMessage response = null;
            try
            {
                response = func.Invoke();
            }
            catch(DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                    }
                }
                LoggError(ex);
                response = requestMessage.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch(DbUpdateException dbEx)
            {
                LoggError(dbEx);
                response = requestMessage.CreateResponse(HttpStatusCode.BadRequest);

            }
            catch(Exception ex)
            {
                LoggError(ex);
                response = requestMessage.CreateResponse(HttpStatusCode.BadRequest);
            }
            return response;
        }

        private void LoggError(Exception ex)
        {
            try
            {
                Error error = new Error();
                error.CreatedDate = DateTime.Now;
                error.Message = ex.Message;
                error.StackTrace = ex.StackTrace;
                _serviceManager.ErrorService.Create(error);
                _serviceManager.ErrorService.SaveChanges();
            }catch
            {

            }
        }    
    }
}
