function AddToCart(productId) {
    var token = document.querySelector('input[name="__RequestVerificationToken"]').value;
    var quantity = document.getElementById("user-addItemCount").value;
    var data = {
        ProductId: productId,
        Quantity: quantity,
        __RequestVerificationToken: token
    }
    $.ajax({
        url: "../Cart/AddToCart",
        type: "POST",
        dataType: "json",
        data: data,
        success: function (data) {
            GetCartItemCount();
            alert("加入購物車成功")
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
