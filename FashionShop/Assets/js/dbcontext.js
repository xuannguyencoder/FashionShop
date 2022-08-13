$(".sweet-confirm").on("click", function () {
    Swal.fire({
        title: 'Bạn có chắc chắc muốn xóa?',
        text: "Bạn sẽ không thể khôi phục nếu xóa",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Xóa',
        cancelButtonText: 'Hủy'
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = $(this).data('href');
        }
    })
});