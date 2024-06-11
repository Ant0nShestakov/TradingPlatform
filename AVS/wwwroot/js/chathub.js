const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chathub")
    .build();

connection.on("ReceiveMessage", function (userId, message) {
    const chat = document.querySelector(".chat");
    const newMessage = document.createElement("li");
    newMessage.className = userId === senderId ? "admin clearfix" : "agent clearfix";
    newMessage.innerHTML = `
        <div class="chat-body clearfix">
            <div class="header clearfix">
                <strong class="primary-font">${userId === senderId ? senderName : recipientName}</strong>
                <small class="right text-muted">
                    <span class="glyphicon glyphicon-time"></span> ${new Date().toLocaleString()}
                </small>
            </div>
            <p>${message}</p>
        </div>
    `;
    chat.appendChild(newMessage);
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

document.querySelector("form").addEventListener("submit", function (event) {
    event.preventDefault();
    const message = document.getElementById("btn-input").value;
    const userId = document.querySelector('input[name="userId"]').value;

    fetch('/Messages/PostMessage?content=' + encodeURIComponent(message) + '&userId=' + encodeURIComponent(userId), {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'X-CSRF-TOKEN': document.querySelector('input[name="__RequestVerificationToken"]').value
        }
    }).then(response => {
        if (response.ok) {
            document.getElementById("btn-input").value = '';
        }
    }).catch(error => console.error('Ошибка при отправке сообщения:', error));
});


const senderId = '@ViewBag.Sender.Id';
const senderName = '@ViewBag.Sender.Name';
const recipientName = '@ViewBag.Recipient.Name';
const recipientId = '@ViewBag.Recipient.Id';
