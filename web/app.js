const apiUrl = "http://localhost:5244/transactions";

async function loadTransactions() {
    try {
        const res = await fetch(apiUrl);
        const data = await res.json();

        const list = document.getElementById("list");
        list.innerHTML = "";

        data.forEach(t => {
            const li = document.createElement("li");
            li.innerHTML = `
                <span style="color: ${t.type === 1 ? "green" : "red"}">
                    ${t.description} - ${t.amount}
                </span>
                <button onclick="deleteTransaction(${t.id})">X</button>
            `;
            list.appendChild(li);
        });

        loadBalance();
    } catch (err) {
        console.error("Erro ao carregar transações:", err);
    }
}

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