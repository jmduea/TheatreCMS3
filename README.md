# Live Project: TheatreCMS Development
## Introduction:
Over the last two weeks of my time at The Tech Academy, I worked on the TheatreCMS project, an ASP.NET MVC and Entity Framework-based content management system (CMS) designed for a theater/acting company. The primary goal of this project was to create an easy-to-use platform for non-technical users to manage website content, productions, subscribers, and maintain a wiki of past performances and performers. The development environment used was Visual Studio 2019, and the application was built on the .NET framework 4.7.2.

### Table of Contents
- [Production Area Contributions](https://github.com/jmduea/TheatreCMS3#production-area)
- [Production Area Code Snippets](https://github.com/jmduea/TheatreCMS3#code-snippets-from-production-area)
- [Blog Area Contributions](https://github.com/jmduea/TheatreCMS3#blog-area)
- [Blog Area Code Snippets](https://github.com/jmduea/TheatreCMS3#code-snippets-from-blog-area)
- [Summary](https://github.com/jmduea/TheatreCMS3#summary)

## Key Contributions:
During this period, I was tasked with working in two areas of the project, the Production area and Blog area. I made several key contributions to both areas of the project during this time. Read on for examples of the contributions I made in each area.

### Production Area:

1. Developed and styled CRUD pages for CalendarEvent and ProductionPhoto models, creating user-friendly interfaces for managing theater events and production images.
2. Implemented event duration and time remaining indicators on the CalendarEvent Index page, providing useful information to the end-users.
3. Added image preview functionality for uploading ProductionPhotos, allowing users to review their selected images before submission.
4. Created an EventPlanner class with role-based restrictions for CRUD operations, ensuring secure access to content management functions.
5. Implemented photo storage and retrieval for ProductionPhotos, converting uploaded images into byte arrays for effecient database storage and retrieval.

### Code in action:
- **Event duration popover and time remaining pill badge implementation:**

![EventDurationGif](https://github.com/jmduea/TheatreCMS3/blob/main/Production/EventDuration.gif)

- **Uploaded image preview functionality:**

![UploadedImagePreviewGif](https://github.com/jmduea/TheatreCMS3/blob/main/Production/UploadedImagePreview.gif)

### Code Snippets from Production Area:

### Blog Area:
 
1. User Interface Enhancements: I updated the UI of the blog comments section, focusing on improving the comment display and interactions, including like/dislike buttons, reply buttons, and delete buttons. I also implemented progress bars for visualizing the like/dislike ratio of comments.
2. AJAX Implementation: To create a smoother user experience, I implemented AJAX for loading, creating, and deleting comments, as well as handling likes and dislikes. This prevents the need for full page reloads when users interact with comments, enhancing overall usability.
3. Reply Functionality: I added the ability for users to reply to comments, with the reply form appearing directly below the parent comment. This feature makes the discussion more engaging and organized.
4. Delete Comment Modal: To prevent accidental deletions, I implemented a confirmation modal that appears when a user clicks the delete button on a comment. Users must confirm their intention to delete the comment before the action is executed.

### Code in action:
- **Creating a comment, updating the like and dislike totals, and progress bar ratio all asynchronously:**

![CreateCommentGif](https://github.com/jmduea/TheatreCMS3/blob/main/Blog/CreateComment.gif)

- **Replying to someone's comment:**

![CreateReplyGif](https://github.com/jmduea/TheatreCMS3/blob/main/Blog/CreateReply.gif)

- **Deleting a comment:**

![DeleteCommentGif](https://github.com/jmduea/TheatreCMS3/blob/main/Blog/DeleteComment.gif)

### Code Snippets from Blog Area:

- **Rendering a comment with Razor syntax:**

The given code is a Razor view template for displaying a single comment and its replies. It performs the following actions:

1. Checks if the Model is not null.
2. Creates a comment container div with a data attribute containing the comment ID.
3. Displays the comment author and time since the comment was posted in the header section.
4. Displays the comment message.
5. Adds action buttons for like, dislike, reply, and delete with their respective icons and counts.
6. Includes a progress bar container to display the like ratio.
7. Creates a container for replies to the comment.
8. If there are any replies to the comment, loops through them and renders each reply using the "_Comment" partial view.

        @model TheatreCMS3.Areas.Blog.Models.Comment

        @if (Model != null)
        {
            <div class="comment-_comments--container" data-comment-id="@Model.CommentId">
                <!-- Header section showing the author and time since the comment was posted -->
                <div class="comment-_comments--header">
                    <span class="comment-author">@((Model.Author != null) ? Model.Author.UserName : "Unknown")</span>
                    <span class="comment-time">- @((DateTime.Now - Model.CommentDate).ToString("dd\\:hh\\:mm")) ago</span>
                </div>
                <!-- Display the comment message -->
                <div class="comment-_comments--message">
                    @Model.Message
                </div>
                <!-- Action buttons for like, dislike, reply, and delete -->
                <div class="comment-_comments--actions">
                    <button class="comment-_comments--btn like-btn">
                        <i class="fas fa-chevron-up"></i> @Model.Likes
                    </button>
                    <button class="comment-_comments--btn dislike-btn">
                        <i class="fas fa-chevron-down"></i> @Model.Dislikes
                    </button>
                    <button class="comment-_comments--replybtn" data-parent-comment-container="$(this).closest('.comment-_comments--container')">Reply</button>
                    <button class="comment-_comments--dangerbtn" data-toggle="modal" data-target="#deleteCommentModal">
                        <i class="fas fa-trash"></i>
                    </button>
                    <!-- Progress bar container below the buttons -->
                    <div class="progress comment-progress bg-danger" id="comment-progress" style="width: 100px;">
                        <div class="progress-bar comment-progress-bar bg-success" id="comment-progress-bar" role="progressbar" style="width: @Model.LikeRatio()%" aria-valuenow="@Model.LikeRatio()" aria-valuemin="0" aria-valuemax="100"></div>
                    </div>
                </div>
                <!-- Container for replies -->
                <div class="comment-replies-@Model.CommentId comment-_comments--replies" id="comment-replies-@Model.CommentId">
                    @if (Model.Replies != null)
                    {
                        foreach (var reply in Model.Replies)
                        {
                            @Html.Partial("_Comment", reply)
                        }
                    }
                </div>
            </div>
        }

- **AJAX request for loading comments asynchronously:**

The loadComments function is responsible for loading comments using an AJAX request. It performs the following steps:

1. Sends an AJAX request to the server with the URL "/Blog/Comments/LoadComments" using the GET method.
2. On successful request completion, it updates the content of the element with the ID "comments-list" with the received result (i.e., the list of comments).
3. If an error occurs during the request, it logs the error message to the console.

        function loadComments() {
            $.ajax({
                url: "/Blog/Comments/LoadComments",
                method: "GET",
                success: function (result) {
                    $("#comments-list").html(result);
                },
                error: function (xhr, status, error) {
                    console.error(error);
                }
            });
        }


- **Handling like and dislike actions:**

The handleVote function takes two arguments: button and actionUrl. This function is responsible for handling both like and dislike actions on a comment. It performs the following steps:
1. It finds the comment container element enclosing the clicked button.
2. It retrieves the comment ID from the data attribute of the comment container.
3. It sends an AJAX request to the server to update the like or dislike count, using the provided actionUrl and passing the commentId as data.
4. Upon a successful AJAX request, it updates the button's HTML with the new like or dislike count, and also updates the progress bar with the new like ratio.

        // This function handles both like and dislike actions
        function handleVote(button, actionUrl) {
         // Get the comment container element that encloses the clicked button
         var commentContainer = button.closest(".comment-_comments--container");
         // Get the comment ID from the data attribute of the comment container
         var commentId = commentContainer.data("comment-id");

         // Send an AJAX request to the server to update the like or dislike count
         $.ajax({
             url: actionUrl,
             method: "POST",
             data: { commentId: commentId },
             success: function (result) {
                 // Update the button HTML with the new count
                 button.html('<i class="fas fa-chevron-' + (actionUrl.includes("Like") ? "up" : "down") + '"></i> ' + (actionUrl.includes("Like") ?
                 result.Likes : result.Dislikes));
                 // Update the progress bar with the new like ratio
                 updateProgressBar(commentContainer, result.LikeRatio);
             }
         });
        }
     
## Summary
