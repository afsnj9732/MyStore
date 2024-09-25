
document.addEventListener('DOMContentLoaded', function () {
    UpdatePrice();
});

function UpdatePrice(){
    $.ajax({
        url: "../Cart/GetTotalPrice",
        type: "Get",
        dataType: "json",
        success: function (data, textStatus, xhr) {
            if (xhr.status == 200) {
                document.getElementById("user-totalPrice").textContent = data.totalPrice;
                var stripe = document.getElementById("user-stripeApi")
                if (stripe != null) {
                    stripe.setAttribute('data-amount', data.totalPrice);
                }
            } 
        },
        error: function (xhr, status, error) {
                console.error("ajax error")
        }
    });
}
function UpdateItem(productId, quantity) {
    var token = document.querySelector('input[name="__RequestVerificationToken"]').value;
    var data = {
        ProductId: productId,
        Quantity: quantity,
        __RequestVerificationToken: token
    }
    $.ajax({
        url: "../Cart/UpdateItem",
        type: "POST",
        dataType: "json",
        data: data,
        success: function (data) {
            GetCartItemCount();
            UpdatePrice();
        },
        error: function (xhr, status, error) {
            if (xhr.status == 401) {
                window.location.href = '../Member/Login';
            } else {
                console.error("ajax error")
            }
        }
    });
}
function DeleteItem(productId) {
    var token = document.querySelector('input[name="__RequestVerificationToken"]').value;
    var data = {
        ProductId: productId,
        __RequestVerificationToken: token
    }
    $.ajax({
        url: "../Cart/DeleteItem",
        type: "POST",
        dataType: "json",
        data: data,
        success: function (data) {
            location.reload();
        },
        error: function (xhr, status, error) {
            if (xhr.status == 401) {
                window.location.href = '../Auth/Login';
            } else {
                console.error("ajax error")
            }
        }
    });
}