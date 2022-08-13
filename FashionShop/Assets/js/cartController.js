var cart = {
    init: function () {
        cart.regEvents();
    },
    regEvents: function () {
        $('#update_cart').off('click').on('click', function (e) {
            e.preventDefault(); //xóa dấu # trên thẻ link
            var listProduct = $('.product_quantity_input');
            var cartList = [];
            $.each(listProduct, function (i, item) {
                cartList.push({
                    Quantity: $(item).val(),
                    ProductDetail: {
                        ID: $(item).data('id'),
                    }
                });
            });
            $.ajax({
                url: '/Cart/Update',
                data: { cartJson: JSON.stringify(cartList) },
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/gio-hang";
                    }
                }
            });
        });
        $('#btnDeleteAll').off('click').on('click', function () {
            $.ajax({
                url: '/Cart/DeleteAll',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/Cart";
                    }
                }
            })
        });
        $('.cart__close').off('click').on('click', function (e) {
            $.ajax({
                data: {
                    productDetailID: $(this).data('id'),
                },
                url: '/Cart/Delete',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/gio-hang";
                    }
                }
            })
        });
    }
}
cart.init();