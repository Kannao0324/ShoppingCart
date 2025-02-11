
window.addEventListener('load', () => {   // Find all the add to cart buttons using the query selector to find all items with the correct class on them
    let addButtons = document.querySelectorAll(".add-item-button");
    // Use Javascript's foreach loop to process each object.
    addButtons.forEach((item) => {
        // Get the Id for the book the butoon is associated with
        let itemId = parseInt(item.getAttribute("value"));
        //Add a listner to the button to run the add to cart method and passes the ID as input parameter.
        item.addEventListener('click', () => addItemToCart(itemId))
    });

})

async function addItemToCart(itemId) {
    let result = await fetch("/ShoppingCart/AddToCart?ItemId=" + itemId);

    if (result.status != 200) {
        location.href = "/Authentication/Login"
    }

    showCartModal();
}
