using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XResize.Bot.Services;

namespace XResize.Bot.Models.Work
{
    public class DocumentJob : Job
    {
        public BotService BotService { get; private set; }
        public TaskQueryService TaskQueryService { get; private set; }
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }

        public DocumentJob(BotService botService, TaskQueryService taskQueryService)
        {
            BotService = botService;
            TaskQueryService = taskQueryService;
        }

        public DocumentJob(BotService botService, 
                           TaskQueryService taskQueryService,
                           string fileId,
                           string fileName,
                           string mimeType,
                           string userName,
                           string userId)
        {
            BotService = botService;
            TaskQueryService = taskQueryService;
            FileId = fileId;
            FileName = fileName;
            MimeType = mimeType;
            UserName = userName;
            UserId = userId;
        }

        public override async Task Execute(SystemInfoService systemInfo)
        {
            var document = await BotService.GetDocument(FileId);
            TaskQueryService.AddNewTask(new ResizeJob(Enums.BotTypeEnum.Telegram, UserName, UserId, document));
        }
    }
}
