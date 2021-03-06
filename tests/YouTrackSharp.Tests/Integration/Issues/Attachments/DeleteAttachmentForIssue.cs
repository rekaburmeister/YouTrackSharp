using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

// ReSharper disable once CheckNamespace
namespace YouTrackSharp.Tests.Integration.Issues
{
    public partial class IssuesServiceTests
    {
        public class DeleteAttachmentForIssue
        {
            [Fact]
            public async Task Valid_Connection_Deletes_Attachment_For_Issue()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                using (var temporaryIssueContext = await TemporaryIssueContext.Create(connection, GetType()))
                {
                    var service = connection.CreateIssuesService();
                    
                    for (var i = 1; i <= 3; i++)
                    {
                        using (var attachmentStream = await TestUtilities.GenerateAttachmentStream("Generated by unit test."))
                        {
                            await service.AttachFileToIssue(temporaryIssueContext.Issue.Id, $"file-{i}.txt", attachmentStream);
                        }
                    }

                    // Act & Assert
                    var acted = false;
                    var attachments = await service.GetAttachmentsForIssue(temporaryIssueContext.Issue.Id);
                    foreach (var attachment in attachments)
                    {
                        // Act & Assert
                        await service.DeleteAttachmentForIssue(temporaryIssueContext.Issue.Id, attachment.Id);
                        acted = true;
                    }

                    Assert.True(acted);

                    await temporaryIssueContext.Destroy();
                }
            }
        }
    }
}