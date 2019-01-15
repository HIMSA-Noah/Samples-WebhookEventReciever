using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Himsa.Samples.Models;
using Himsa.Samples.Models.Common;
using Himsa.Samples.Models.Interface;
using Himsa.Samples.NoahEventProcessor;

namespace Himsa.Samples.WebHookEventReciever.Controllers
{
    [Route("api/[controller]")]
    public class NoahEventsController : Controller
    {
        protected INoahEventQueue _noahEventQueue;
        private readonly ILogger _logger;

        private readonly AppSettings _settings;

        public NoahEventsController(IOptions<AppSettings> options, INoahEventQueue noahEventQueue, ILogger<NoahEventsController> logger)
        {
            _noahEventQueue = noahEventQueue;
            _logger = logger;

            _settings = options.Value;
        }

        /// <summary>
        /// This action is used for handling the verification request. 
        /// The purpose of this verification request is to demonstrate that your app really does want to receive notifications at that URI
        /// </summary>
        /// <param name="challenge">The chalange submitted by the publisher</param>
        /// <returns>the challenge is echoed back to the caller</returns>
        public IActionResult Get([FromQuery] string challenge = null) {
            
            if (string.IsNullOrEmpty(challenge))
            {
                return Ok();
            }
            else
            {
                _logger.LogInformation($"Noah Event Challenge Recieved OK, Challenge: {challenge}");
                // In order to avoid introducing a reflected XSS vulnerability, the following headers are set in the response to the verification request
                Response.Headers.Add("Content-Type", "text/plain");
                Response.Headers.Add("X-Content-Type-Options", "nosniff");
                return Ok(challenge);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] NoahEvent noahEvent)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string body = "";
                    Request.EnableRewind();
                    using (var reader = new StreamReader(Request.Body))
                    {
                        Request.Body.Seek(0, SeekOrigin.Begin);
                        body = reader.ReadToEnd();
                    }

                    string signatureFromHeader = Request.Headers["X-Hub-Signature"].FirstOrDefault();
                    string originFromHeader = Request.Headers["X-Hub-Origin"].FirstOrDefault();
                    string transmissionAtemptFromHeader = Request.Headers["X-Hub-TransmissionAtempt"].FirstOrDefault();

                    string sharedSecret = _settings.WebHookSharedSecret; // "keepmesafe";

                    bool isSignatureValid = SignatureValidationUtil.ValidateSignature(body, signatureFromHeader, sharedSecret);

                    if (isSignatureValid == false)
                    {
                        _logger.LogError($"Noah Event Recieved was rejected - invalid signature, TenantId: {noahEvent.TenantId}, NotificationEventId: {noahEvent.NotificationEventId}, SignatureFromHeader: {signatureFromHeader}");
                        return Unauthorized();
                    }

                    _noahEventQueue.Enqueue(noahEvent);
                    _logger.LogInformation($"Noah Event Recieved OK, TenantId: {noahEvent.TenantId}, NotificationEventId: {noahEvent.NotificationEventId}");
                    return Ok();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Noah Event Recieved Unable to process incoming event, TenantId: {noahEvent.TenantId}, NotificationEventId: {noahEvent.NotificationEventId}");
                    return BadRequest("Unable to process incoming event");
                }
            }
            else
            {
                _logger.LogError($"Noah Event Recieved Invalid Message, TenantId: {noahEvent.TenantId}, NotificationEventId: {noahEvent.NotificationEventId}");
                return BadRequest(ModelState);
            }
        }
        
    }
}