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

// This function updates the progress bar width and aria-valuenow attribute
function updateProgressBar(commentContainer, likeRatio) {
    // Find the progress bar element within the direct children of the comment container
    var progressBar = commentContainer.children('.comment-_comments--actions').find('.progress-bar');
    // Set the width of the progress bar based on the like ratio percentage
    progressBar.css("width", likeRatio + "%");
    // Update the aria-valuenow attribute with the like ratio value
    progressBar.attr("aria-valuenow", likeRatio);
}

// This function handles both like and dislike actions
function handleVote(button, actionUrl) {
    console.log("handleVote called with actionUrl:", actionUrl);
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
            console.log("Ajax request successful, result:", result);
            // Update the button HTML with the new count
            button.html('<i class="fas fa-chevron-' + (actionUrl.includes("Like") ? "up" : "down") + '"></i> ' + (actionUrl.includes("Like") ? result.Likes : result.Dislikes));
            // Update the progress bar with the new like ratio
            updateProgressBar(commentContainer, result.LikeRatio);
        }
    });
}

//This function shows the delete success message after a comment is deleted
function showDeleteSuccessMessage(message) {
    var successMessage = $(".comment-deleted-message");
    successMessage.html(message + ' <i class="fas fa-check"></i>');
    successMessage.fadeIn(500, function () {
        successMessage.delay(3000).animate({ opacity: 0 }, 500, function () {
            successMessage.css('opacity', 1).hide();
        });
    });
}

// This function deletes a comment using AJAX and updates the UI
function deleteComment(button) {
    var commentContainer = button.closest(".comment-_comments--container");
    var commentId = commentContainer.data("comment-id");
    console.log("commentId: " + commentId); // Ensure this line is present for debugging

    // Get the Anti-Forgery token
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: "/Blog/Comments/Delete",
        method: "POST",
        data: {
            id: commentId,
            __RequestVerificationToken: token
        },
        success: function (result) {
            console.log(result); // Add this line for debugging
            commentContainer.fadeOut(500, function () {
                commentContainer.remove();
                showDeleteSuccessMessage(result.message);
                loadComments();
            })
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}


// The document ready event ensures that the DOM is fully loaded before executing the code
$(document).ready(function () {
    $("#create-reply-form").hide();
    // Use event delegation to attach a click event listener to the like buttons
    $(document).on("click", ".like-btn", function () {
        console.log("Like button clicked");
        handleVote($(this), "/Blog/Comments/Like");
    });

    // Use event delegation to attach a click event listener to the dislike buttons
    $(document).on("click", ".dislike-btn", function () {
        console.log("Dislike button clicked");
        handleVote($(this), "/Blog/Comments/Dislike");
    });

    // Attach a click event listener to the delete buttons
    $(document).on("click", ".comment-_comments--dangerbtn", function () {
        var commentContainer = $(this).closest(".comment-_comments--container");
        // Store the reference to the comment container in the confirm delete button's data attribute
        $(".confirm-delete-btn").data("comment-container", commentContainer);
    });

    // Attach a click event listener to the confirm delete button in the modal
    $(document).on("click", ".confirm-delete-btn", function () {
        // Retrieve the reference to the comment container from the confirm delete button's data attribute
        var commentContainer = $(this).data("comment-container");
        deleteComment(commentContainer);
        // Hide the delete confirmation modal
        $("#deleteCommentModal").modal("hide");
    });

    $(document).on("click", ".comment-_comments--replybtn", function () {
        var commentContainer = $(this).closest(".comment-_comments--container");
        var commentId = commentContainer.data("comment-id");
        console.log("commentContainer:", commentContainer);
        // Move the comment form below the clicked comment
        $("#create-reply-form").insertAfter(commentContainer);
        $("#create-reply-form").show();

        // Set the parentCommentId input value to the clicked comment's ID
        $('input[name="parentCommentId"]').val(commentId);

        // Set the parent comment container data
        $("#create-reply-form").data("parent-comment-container", commentContainer);
    });

    $(document).on("submit", "#create-reply-form", function (event) {
        event.preventDefault();

        var form = $(this);
        var message = form.find('textarea[name="message"]').val();
        var parentCommentId = form.find('input[name="parentCommentId"]').val();
        var token = $('input[name="__RequestVerificationToken"]', form).val();

        $.ajax({
            url: '/Comments/CreateReply',
            method: 'POST',
            data: {
                Message: message,
                ParentCommentId: parentCommentId,
                __RequestVerificationToken: token
            },
            success: function (result) {
                // Clear the comment form and reset the parentCommentId input value
                form.find('textarea[name="message"]').val('');
                form.hide();

                // Append the new comment HTML
                var newCommentHtml = `
    <div class="comment-_comments--container" data-comment-id="${result.commentId}">
        <div class="comment-_comments--header">
            <span class="comment-author">${result.author}</span>
            <span class="comment-time">- ${result.timeAgo} ago</span>
        </div>
        <div class="comment-_comments--message">${message}</div>
        <div class="comment-_comments--actions">
            <button class="comment-_comments--btn like-btn">
                <i class="fas fa-chevron-up"></i> <span class="likes-count">0</span>
            </button>
            <button class="comment-_comments--btn dislike-btn">
                <i class="fas fa-chevron-down"></i> <span class="dislikes-count">0</span>
            </button>
            <button class="comment-_comments--replybtn" data-parent-comment-container="$(this).closest('.comment-_comments--container')">Reply</button>
            <button class="comment-_comments--dangerbtn" data-toggle="modal" data-target="#deleteCommentModal">
                <i class="fas fa-trash"></i>
            </button>
            <!-- Progress bar container below the buttons -->
            <div class="progress comment-progress bg-danger" style="width: 100px;">
                <div class="progress-bar bg-success" role="progressbar" style="width: 0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100"></div>
            </div>
        </div>
    </div>`;

                // Find the parent comment container and its replies container
                var parentCommentId = form.find('input[name="parentCommentId"]').val();
                var parentCommentReplies = $("#comment-replies-" + parentCommentId);

                // Append the new comment to the parent comment's replies container
                parentCommentReplies.append(newCommentHtml);

            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });

    $("#create-comment-form").on("submit", function (event) {
        event.preventDefault();

        var message = $(this).find('textarea[name="message"]').val();
        var token = $('input[name="__RequestVerificationToken"]').val();

        $.ajax({
            url: "/Blog/Comments/Create",
            method: "POST",
            data: {
                Message: message,
                __RequestVerificationToken: token
            },
            success: function (result) {
                // Clear the comment form
                $("#create-comment-form").find('textarea[name="message"]').val('');
                loadComments();
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });
    loadComments();
});