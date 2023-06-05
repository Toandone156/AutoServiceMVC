var bookButton = document.querySelector(".bookbutton");
var inputCode = document.querySelector(".tablecode");

bookButton.addEventListener("click", e => {
    e.preventDefault();
    accessTableAjax(inputCode.value);
})

inputCode.addEventListener("keyup", e => {
    if (e.key === "Enter") {
        accessTableAjax(inputCode.value);
    }
})