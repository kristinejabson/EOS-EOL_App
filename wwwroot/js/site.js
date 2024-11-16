const baseUrl = window.location.origin;

const backBtn = document.getElementById('back-btn');
const notifBtn = document.getElementById('notif-btn');
const notifContainer = document.getElementById('notif-container');

$(document).ready(function () {
    const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]')
    const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl))
});

function sendAjaxRequest(url, method, data, successCallback = defaultSuccess, errorCallback = defaultError, options = {}) {
    var ajaxOptions = {
        url: url,
        type: method,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data),
        success: successCallback,
        error: errorCallback,
    };
    $.extend(ajaxOptions, options);
    $.ajax(ajaxOptions);
}

//Will go to hardware details for now
function viewNotification() {
    window.location.href = `${baseUrl}/Hardware/HardwareDetails`;
}

notifBtn.onclick = function (event) {
    event.stopPropagation();
    sendAjaxRequest(
        `${baseUrl}/Notification/NotificationListPartial?isPartial=true`,
        'GET',
        {},
        (data) => {
            notifContainer.classList.toggle('visible');

            notifContainer.innerHTML = data;
        },
        (data) => {
            alert(baseUrl)
        }
    );
}

document.addEventListener('click', function (event) {
    if (!notifContainer.contains(event.target)) {
        //Do not toggle if already not visible
        if (notifContainer.classList.contains('visible')) {
            notifContainer.classList.toggle('visible');
        }
    }
});

backBtn.onclick = function (event) {
    event.stopPropagation();
    window.history.back();
}