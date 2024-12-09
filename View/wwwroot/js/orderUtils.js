// orderUtils.js

// Cập nhật tổng tiền trong bảng và toàn bộ đơn hàng
function updateTotalPriceInTable(productId, quantity, price) {
    const totalPriceElement = document.querySelector(`tr[data-id="${productId}"] .total-price`);
    if (totalPriceElement) {
        const newTotalPrice = quantity * price;
        totalPriceElement.textContent = newTotalPrice.toFixed(2);
    }
    updateOrderTotal();
}

// Hàm tính tổng giá trị toàn bộ đơn hàng
function updateOrderTotal() {
    const totalElements = document.querySelectorAll('.total-price');
    let totalOrderPrice = 0;

    totalElements.forEach(element => {
        const price = parseFloat(element.textContent);
        if (!isNaN(price)) {
            totalOrderPrice += price;
        }
    });

    const orderTotalElement = document.getElementById("orderTotal");
    if (orderTotalElement) {
        orderTotalElement.textContent = totalOrderPrice.toFixed(2);
    }
}
