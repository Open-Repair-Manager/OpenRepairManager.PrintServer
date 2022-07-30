using Grpc.Core;
using RepairCafeBassendeanSessionServer;
using bpac;

namespace RepairCafeBassendeanSessionServer.Services
{
    public class PrintItemService : PrintItem.PrintItemBase
    {
        private readonly ILogger<PrintItemService> _logger;
        //private const string TEMPLATE_DIRECTORY = @"C:\RCBAPP\Templates\"; // Template file path
        //private const string TEMPLATE_SIMPLE = "testrcb.lbx";    // Template file name

        public PrintItemService(ILogger<PrintItemService> logger)
        {
            _logger = logger;
        }

        public override Task<PrintReply> PrintItem(RepairItemRequest request, ServerCallContext context)
        {
            string templatePath = "RCB_TBA_LABEL.lbx";
            //templatePath += TEMPLATE_SIMPLE;// None decoration frame
            string result;

            bpac.Document doc = new();
            if (doc.Open(templatePath) != false)
            {
                doc.GetObject("objCustName").Text = request.Owner;
                doc.GetObject("objItem").Text = request.Item;
                doc.GetObject("objItemNo").Text = request.Id;

                // doc.SetMediaById(doc.Printer.GetMediaId(), true);
                doc.StartPrint("", PrintOptionConstants.bpoDefault);
                doc.PrintOut(1, PrintOptionConstants.bpoDefault);
                doc.EndPrint();
                doc.Close();
                _logger.LogInformation("Successfully printed item");
                result = "Success!";
            }
            else
            {
                _logger.LogWarning("Error printing: " + doc.ErrorCode);
                result = "Error printing: " + doc.ErrorCode;
            }

            return Task.FromResult(new PrintReply
            {
                Messages = result
            });
        }
    }
}
