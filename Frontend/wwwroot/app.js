// --- LOGIKA PRZEŁĄCZANIA ZAKŁADEK ---
const menuLinks = document.querySelectorAll('.sidebar ul li a');
const views = document.querySelectorAll('.view');

menuLinks.forEach(link => {
    link.addEventListener('click', function (e) {
        e.preventDefault();

        menuLinks.forEach(l => l.classList.remove('active'));
        this.classList.add('active');

        // Określamy docelowy identyfikator sekcji (ID) na podstawie klikniętego tekstu
        const clickedText = this.innerText.trim();
        let targetViewId = "";

        if (clickedText === "Budżet") {
            targetViewId = "view-budget";
        } else if (clickedText === "Goście") {
            targetViewId = "view-guests";
        } else if (clickedText === "Harmonogram") {
            targetViewId = "view-harmonogram";
        }

        // Ukrycie wszystkich widoków i pokazanie tylko tego dopasowanego
        if (targetViewId !== "") {
            views.forEach(v => v.classList.remove('active'));
            document.getElementById(targetViewId).classList.add('active');
        }

        if (targetViewId === "view-guests") {
            // console.log("Pobieram listę gości...");
            // fetchGuestsList(); 
        } else if (targetViewId === "view-harmonogram") {
            // console.log("Pobieram harmonogram...");
            // fetchScheduleEvents();
        }
    });
});

// Obsługa wysyłania nowego wydatku
document.getElementById('add-expense-form').addEventListener('submit', function (event) {
    event.preventDefault();

    const dateInput = document.getElementById('expense-date');
    const dateValue = dateInput && dateInput.value ? new Date(dateInput.value).toISOString() : null;

    const notesInput = document.getElementById('expense-notes');
    const notesValue = notesInput ? notesInput.value : "";

    const newExpense = {
        name: document.getElementById('expense-name').value,
        amount: parseFloat(document.getElementById('expense-amount').value),
        category: document.getElementById('expense-category').value,
        isPaid: false,
        paymentDate: dateValue,
        notes: notesValue,
        advanceAmount: 0
    };

    // Wysyłamy żądanie POST do Bramy API
    fetch('http://localhost:5000/api/Expenses', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(newExpense)
    })
        .then(response => {
            if (response.ok) {
                document.getElementById('add-expense-form').reset();
                //Odświeżenie wszystkiego
                fetchExpensesList();
            } else {
                alert("Wystąpił błąd podczas dodawania wydatku.");
            }
        })
        .catch(error => console.error('Błąd:', error));
});

// Funkcja pobierająca listę, obliczająca sumę i generująca kategorie
function fetchExpensesList() {
    fetch('http://localhost:5000/api/Expenses')
        .then(response => response.json())
        .then(data => {
            const tbody = document.getElementById('expenses-list');
            tbody.innerHTML = '';

            let totalCost = 0;
            const categorySums = {}; 

            if (data.length === 0) {
                tbody.innerHTML = '<tr><td colspan="7" style="text-align: center;">Brak wydatków w budżecie.</td></tr>';
                document.getElementById('total-cost').innerText = "0 zł";
                document.getElementById('category-list').innerHTML = '<li>Brak danych</li>';
                return;
            }

            data.forEach(expense => {
                // obliczanie kosztów
                totalCost += expense.amount;

                if (categorySums[expense.category]) {
                    categorySums[expense.category] += expense.amount;
                } else {
                    categorySums[expense.category] = expense.amount;
                }

                // generowanie wierszy do tabeli
                const tr = document.createElement('tr');

                const statusHtml = expense.isPaid
                    ? '<span class="status-badge status-paid">Opłacone</span>'
                    : '<span class="status-badge status-unpaid">Do zapłaty</span>';

                const dateText = expense.paymentDate ? new Date(expense.paymentDate).toLocaleDateString('pl-PL') : '-';
                const notesText = expense.notes ? expense.notes : '-';

                const actionBtn = !expense.isPaid
                    ? `<button type="button" class="btn-action" onclick="markAsPaid(${expense.id})">✔ Zapłać</button>`
                    : `<span style="font-size: 12px; color: #7f8c8d;">Brak akcji</span>`;

                tr.innerHTML = `
                    <td><strong>${expense.name}</strong></td>
                    <td>${expense.category}</td>
                    <td>${dateText}</td>
                    <td><strong style="color: #4A3A31;">${expense.amount} zł</strong></td>
                    <td><small>${notesText}</small></td>
                    <td>${statusHtml}</td>
                    <td>${actionBtn}</td>
                `;

                tbody.appendChild(tr);
            });


            document.getElementById('total-cost').innerText = totalCost + " zł";

            const catList = document.getElementById('category-list');
            catList.innerHTML = '';
            for (const [catName, sum] of Object.entries(categorySums)) {
                catList.innerHTML += `<li>${catName} <span>${sum} zł</span></li>`;
            }
        })
        .catch(error => console.error('Błąd pobierania listy:', error));
}

// Funkcja zmieniająca status wydatku na Opłacony
function markAsPaid(id) {
    if (!confirm("Czy na pewno chcesz oznaczyć ten wydatek jako zapłacony?")) return;

    fetch(`http://localhost:5000/api/Expenses`)
        .then(response => response.json())
        .then(allExpenses => {
            const expense = allExpenses.find(e => e.id === id);

            if (!expense) return;

            expense.isPaid = true;

            return fetch(`http://localhost:5000/api/Expenses/${id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(expense)
            });
        })
        .then(response => {
            if (response.ok) {
                fetchExpensesList();
            } else {
                alert("Błąd podczas aktualizacji statusu.");
            }
        })
        .catch(error => console.error('Błąd:', error));
}

fetchExpensesList();