# Live Project: TheatreCMS Development
## Introduction:
Over the last two weeks of my time at The Tech Academy, I worked on the TheatreCMS project, an ASP.NET MVC and Entity Framework-based content management system (CMS) designed for a theater/acting company. The primary goal of this project was to create an easy-to-use platform for non-technical users to manage website content, productions, subscribers, and maintain a wiki of past performances and performers. The development environment used was Visual Studio 2019, and the application was built on the .NET framework 4.7.2.

### Table of Contents
- [Production Area Contributions](https://github.com/jmduea/TheatreCMS3/blob/main/README.md#production-area)
- [Production Area Code Snippets](https://github.com/jmduea/TheatreCMS3#code-snippets-from-production-area)
- [Blog Area Contributions](https://github.com/jmduea/TheatreCMS3#code-snippets-from-production-area)
- [Blog Area Code Snippets](https://github.com/jmduea/TheatreCMS3#code-snippets-from-production-area)
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
- Event duration popover and time remaining pill badge implementation:

![EventDurationGif](https://github.com/jmduea/TheatreCMS3/blob/main/Production/EventDuration.gif)

- Uploaded image preview functionality:

![UploadedImagePreviewGif](https://github.com/jmduea/TheatreCMS3/blob/main/Production/UploadedImagePreview.gif)

### Code Snippets from Production Area:

### Blog Area:
 
1. User Interface Enhancements: I updated the UI of the blog comments section, focusing on improving the comment display and interactions, including like/dislike buttons, reply buttons, and delete buttons. I also implemented progress bars for visualizing the like/dislike ratio of comments.
2. AJAX Implementation: To create a smoother user experience, I implemented AJAX for loading, creating, and deleting comments, as well as handling likes and dislikes. This prevents the need for full page reloads when users interact with comments, enhancing overall usability.
3. Reply Functionality: I added the ability for users to reply to comments, with the reply form appearing directly below the parent comment. This feature makes the discussion more engaging and organized.
4. Delete Comment Modal: To prevent accidental deletions, I implemented a confirmation modal that appears when a user clicks the delete button on a comment. Users must confirm their intention to delete the comment before the action is executed.

### Code in action:
- Creating a comment, updating the like and dislike totals, and progress bar ratio all asynchronously:

![CreateCommentGif](https://github.com/jmduea/TheatreCMS3/blob/main/Blog/CreateComment.gif)

- Replying to someone's comment:

![CreateReplyGif](https://github.com/jmduea/TheatreCMS3/blob/main/Blog/CreateReply.gif)

- Deleting a comment:

![DeleteCommentGif](https://github.com/jmduea/TheatreCMS3/blob/main/Blog/DeleteComment.gif)

### Code Snippets from Blog Area:

## Summary
