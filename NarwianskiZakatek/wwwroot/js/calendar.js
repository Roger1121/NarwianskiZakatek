let offset = 0;

const calendar = document.getElementById('calendar');
const header = document.getElementById('calendar_header');
const weekdays = ['poniedziałek', 'wtorek', 'środa', 'czwartek', 'piątek', 'sobota', 'niedziela'];

function load() {
    const date = new Date();
    const day = date.getDate();

    const firstDayOfMonth = new Date(date.getFullYear(), date.getMonth(), 1);
    const daysInMonth = new Date(date.getFullYear(), date.getMonth() + 1, 0).getDate();
    const weekday = firstDayOfMonth.toLocaleDateString('pl-pl', { weekday: 'long' });
    const paddingDays = weekdays.indexOf(weekday);

    const dateString = date.toLocaleDateString('pl-pl', {
        weekday: 'long',
        day: 'numeric',
        month: 'numeric',
        year: 'numeric',
    });

    for (let i = 1; i <= paddingDays + daysInMonth; i++) {
        const daySquare = document.createElement('div');
        daySquare.classList.add('day');

        if (day + paddingDays == i) {
            daySquare.setAttribute('id', 'currentDay');
        }

        if (i <= paddingDays) {
            daySquare.classList.add('padding');
        }
        else {
            daySquare.innerText = i - paddingDays;
        }

        calendar.appendChild(daySquare);
    }
    header.innerText = dateString;
}

load();