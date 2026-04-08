const apiUrl = "http://localhost:5244/transactions";

let currentPage = 1;
let currentPageSize = 10;

async function loadTransactions(page = currentPage, pageSize = currentPageSize) {
    try {
        const res = await fetch(`${apiUrl}?Page=${page}&PageSize=${pageSize}`);
        const json = await res.json();

        const transactions = json.data;

        const list = document.getElementById("list");
        list.innerHTML = "";

        transactions.forEach(t => {
            const li = document.createElement("li");
            li.innerHTML = `
                <span style="color: ${t.type === 1 ? "green" : "red"}">
                    ${t.description} - ${t.amount}
                </span>
                <button onclick="deleteTransaction(${t.id})">X</button>
            `;
            list.appendChild(li);
        });

        currentPage = json.page;
        currentPageSize = json.pageSize;
        const totalPages = Math.ceil(json.total / json.pageSize);

        document.getElementById("pageInput").value = currentPage;
        document.getElementById("pageSizeInput").value = currentPageSize;
        document.getElementById("pageInfo").textContent = `Página ${currentPage} de ${totalPages}`;

        document.getElementById("prevBtn").disabled  = currentPage <= 1;
        document.getElementById("nextBtn").disabled  = currentPage >= totalPages;

        loadBalance();
    } catch (err) {
        console.error("Erro ao carregar transações:", err);
    }
}

document.getElementById("prevBtn").addEventListener("click", () => {
    if (currentPage > 1) loadTransactions(currentPage - 1, currentPageSize);
});

document.getElementById("nextBtn").addEventListener("click", () => {
    loadTransactions(currentPage + 1, currentPageSize);
});

document.getElementById("goBtn").addEventListener("click", () => {
    const page = parseInt(document.getElementById("pageInput").value) || 1;
    const pageSize = parseInt(document.getElementById("pageSizeInput").value) || 10;
    loadTransactions(page, pageSize);
});

async function createTransaction() {
    const description = document.getElementById("description").value;
    const amount = parseFloat(document.getElementById("amount").value);
    const type = parseInt(document.getElementById("type").value);

    if (!description || amount <= 0) {
        alert("Preencha os dados corretamente");
        return;
    }

    await fetch(apiUrl, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            description,
            amount,
            date: new Date().toISOString(),
            type,
            category: "General"
        })
    });

    document.getElementById("description").value = "";
    document.getElementById("amount").value = "";

    loadTransactions();
}

async function deleteTransaction(id) {
    await fetch(`${apiUrl}/${id}`, {
        method: "DELETE"
    });

    loadTransactions();
}

async function loadBalance() {
    const res = await fetch(`${apiUrl}/balance`);
    const data = await res.json();

    const balanceEl = document.getElementById("balance");

    balanceEl.textContent = `
        Income: ${data.income} | 
        Expense: ${data.expense} | 
        Balance: ${data.balance}
    `;
}