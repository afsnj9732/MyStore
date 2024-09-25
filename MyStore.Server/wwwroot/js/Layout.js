document.addEventListener('DOMContentLoaded', function () {
    GetCartItemCount();
});
function GetCartItemCount() {
    $.ajax({
        url: "../Cart/GetCartItemCount",
        type: "GET",
        dataType: "json",
        success: function (data, textStatus, xhr) {
            if (xhr.status == 200) {
                    document.getElementById("CartItemCount").textContent = data.cartItemCount;
            }
        },
        error: function (xhr, status, error) {
            console.error("ajax error")
        }
    });
}


