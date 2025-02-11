
window.addEventListener('load', () => {

    document.getElementById('btnCartModal').addEventListener('click', () => showCartModal());
});

async function showCartModal() {
    //Request the current users cart in the partial view from the cart controller
    var result = await fetch("/ShoppingCart/Index");
    // If the request fails, redirect the user to the login
    if (result.status != 200) {
        location.href = "/Authentication/Login";
    }
    //Get the partial view's HTML from the body of the result
    var htmlResult = await result.text();
    //Find the cartModalBody section of the modal and put the HTML inside its tag
    document.getElementById("cartModalBody").innerHTML = htmlResult;
    //Find all the forms that hold the plus and minus buttons
    let itemQtyForms = document.querySelectorAll(".cart-qty-toggle");
    //Cycle through each one and add a listner to disable its submit behaviour
    itemQtyForms.forEach((item) => {
        item.addEventListener('submit', (e) => {
            e.preventDefault()
        })
    });

    setupQuantityButtons();
    setupRemoveButtons();
    setupCancelAndCheckoutButtons();

    calculateCartTotal();


    //Tell the modal to show the screen 
    $("#cartModal").modal("show");
}

async function setupQuantityButtons() {
    //Find all the buttons with the btn-minus class on them
    let minusButtons = document.querySelectorAll(".btn-minus");
    //Cycle through the buttons and add event lisnters to each one
    minusButtons.forEach((item) => {
        //Each event triggers a method call with takes the event data and the amount change the number by
        item.addEventListener('click', (e) => changeQuantity(e, -1))
    })

    //Find all the buttons with the btn-plus class on them
    let plusButtons = document.querySelectorAll(".btn-plus");
    //Cycle through the buttons and add event lisnters to each one
    plusButtons.forEach((item) => {
        //Each event triggers a method call with takes the event data and the amount change the number by
        item.addEventListener('click', (e) => changeQuantity(e, 1))
    })
}

async function changeQuantity(e, amount) {
    //Find the input field that is in the form of the button that was pressed.
    let cartItemId = parseInt(e.target.form.querySelector('input').value);
    //Find the qty text field that is in the form of the button that was pressed.
    let qty = parseInt(e.target.form.querySelector('.qty-text').innerText);
    //Change the quantity by the provided amount
    qty += amount;
    //If the quantity will drop to 0 or less.
    if (qty <= 0) {
        removeItem(e, cartItemId);
        return;
    }
    //Find the qty text field in the current item and change its text to the new quantity value
    e.target.form.querySelector('.qty-text').innerText = qty;

    recalculateLineTotal(e, qty);
    calculateCartTotal();
    //Pass the new quantity and cart item id to the next method for updating the database.
    updateQuantityInDatabase(qty, cartItemId);
}

async function functionupdateQuantityInDatabase(qty, cartItemId) {
    //Create a javaScript object to hold our values
    let updatedItem = {
        Quantity: qty,
        Id: cartItemId
    };
    //Send a fetch request to the server to update the quantity in the database
    let response = await fetch("/ShoppingCart/UpdateQuantity", {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(updatedItem)
    });
}

async function setupRemoveButtons() {
    //Find all the remove buttons in the shopping cart
    let removeButtons = document.querySelectorAll(".remove-button");
    //Cycle through each button in a foreach loop for processing.
    removeButtons.forEach((item) => {
        //Get the item Id from the value attribute in the button
        let cartItemId = parseInt(item.getAttribute('value'));
        //Add a listener to the button to run the remove method and pass it the event and ID values.
        item.addEventListener('click', (e) => removeItem(e, cartItemId));
    });
}

async function removeItem(e, cartItemId) {
    let result = await fetch("/ShoppingCart/RemoveFromCart?id=" + cartItemId, {
        method: "DELETE"
    })

    if (result.status == 200) {
        //Get the button that triggered the event
        let source = e.target;
        //Find the closest parent node with the card class on it
        let parent = source.closest(".card")
        //Remove the parent component form the document.
        parent.remove();
        calculateCartTotal();
    }
    else {
        alert("Something went wrong!");
    }

}

async function recalculateLineTotal(e, qty) {
    //Get the element that holds the line total for the current item
    let lineItem = e.target.form.querySelector(".line-total");
    //Get the single unit price for the item the line item's value attribute
    let unitPrice = parseFloat(lineItem.getAttribute('value'));
    //Calculate the new total. We will need to use the Number() type here to maintain accuracy
    let totalPrice = Number(qty * unitPrice);
    //Pass the new value back into the line total element
    e.target.form.querySelector(".line-total").innerText = "Total: $" + totalPrice.toFixed(2);

}

async function calculateCartTotal() {
    //Create a variable to hold our total and set it to a float
    let total = 0.00;
    //Find all the line total elements on the page
    let lineItems = document.querySelectorAll(".line-total");
    //If there are currently no items in the cart
    if (lineItems.length == 0) {
        //Remove the Checkout and cancel buttons.
        document.querySelector("#btnCheckout").remove();
        document.querySelector("#btnCancel").remove();
    }

    //Cycle through each line item
    lineItems.forEach((item) => {
        //Get the current item line's content and split it on the dollar sign and store the 2nd element
        let linePrice = parseFloat(item.innerHTML.split("$")[1]);
        //Add the current item to the line price
        total += linePrice;
    });
    //Find the cart total field and update its HTML to show the new value
    document.querySelector("#modalGrandTotal").innerHTML = "$" + total.toFixed(2) + " <strong>.inc GST</strong>";
}

async function setupCancelAndCheckoutButtons() {
    //Find the checkpout button by using its ID
    let checkoutButton = document.querySelector("#btnCheckout");
    //If the button is not null, add a listner to run the desired method
    if (checkoutButton != null) {
        checkoutButton.addEventListener('click', (e) => finaliseCart(e));
    }

    //Find the cancel button by using its ID
    let cancelButton = document.querySelector("#btnCancel");
    //If the button is not null, add a listner to run the desired method
    if (cancelButton != null) {
        cancelButton.addEventListener('click', (e) => cancelCart(e));
    }
}

async function finaliseCart(e) {
    if (confirm("Go To Checkout?") == true) {   //Get the ID out of the button that was pressed
        let id = parseInt(e.target.getAttribute("value"));
        //Send a fetch result to process the checkout in the server
        let result = await fetch("/ShoppingCart/FinaliseCart?id=" + id);

        if (result.status == 200) {
            $('#cartModal').modal('hide');

            alert("Cart Finalised.");
           
        }
        else {
            alert("Something Went Wrong");
        }
    }
    else {
        return;
    }
}




async function cancelCart(e) {
    if (confirm("Are you sure you wish to cancel your purchase?") == true) {   //Get the ID out of the button that was pressed
        let id = parseInt(e.target.getAttribute("value"));
        //Send a fetch result to process the checkout in the server
        let result = await fetch("/ShoppingCart/CancelCart/" + id);

        if (result.status == 200) {
            $('#cartModal').modal('hide');

            alert("Cart Cancelled.");
        }
        else {
            alert("Something Went Wrong");
        }
    }
    else {
        return;
    }
}