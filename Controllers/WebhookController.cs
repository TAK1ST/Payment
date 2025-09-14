using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;

namespace Payment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhookController : ControllerBase
    {
        private readonly PayOS _payOS;

        public WebhookController(PayOS payOS)
        {
            _payOS = payOS;
        }

        [HttpPost("payos")]
        public IActionResult Receive([FromBody] WebhookType payload)
        {
            // Verify signature, dữ liệu
            WebhookData wd;

            try
            {
                wd = _payOS.verifyPaymentWebhookData(payload);
            }
            catch (Exception ex)
            {
                // nếu verify fail
                return BadRequest("Invalid signature");
            }

            // Xử lý wd: wd.OrderCode, wd.Amount, wd.TransactionDateTime, etc.
            // Cập nhật database, gửi mail, etc.

            return Ok(new { success = true });
        }
    }
}
