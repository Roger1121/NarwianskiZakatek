function get_icon_uri(weather_code) {
    switch (weather_code) {
        case 200:
        case 201:
        case 202:
        case 230:
        case 231:
        case 232:
        case 233:
            return '/graphics/weather/thunderstorm.svg';
        case 300:
        case 301:
        case 302:
        case 500:
        case 520:
            return '/graphics/weather/small_rain.svg';
        case 501:
        case 502:
        case 511:
        case 521:
        case 522:
            return '/graphics/weather/heavy_rain.svg';
        case 601:
        case 602:
        case 621:
        case 622:
        case 623:
            return '/graphics/weather/snow.svg';
        case 610:
        case 611:
        case 612:
            return '/graphics/weather/snow_and_rain.svg';
        case 800:
            return '/graphics/weather/sun.svg';
        case 801:
        case 802:
            return '/graphics/weather/sun_with_clouds.svg';
        case 803:
        case 804:
        case 900:
            return '/graphics/weather/clouds.svg';
        default:
            return '/graphics/weather/fog.svg';
    }
}

function set_weather_today() {

    const url = 'https://api.weatherbit.io/v2.0/current?key=b729919c22744e40a686085b39ad7038&lang=pl&city=Bialystok';
    const weatherIcon = document.getElementById('icon_today');
    const temperature = document.getElementById('temperature_today');
    const description = document.getElementById('description_today');
    const date = document.getElementById('date_today');

    fetch(url)
        .then(response => response.json())
        .then(data => {

            weatherIcon.setAttribute('src', get_icon_uri(data.data[0].weather.code));
            temperature.innerText = data.data[0].temp + "℃";
            description.innerText = data.data[0].weather.description;
            date.innerText = new Date().toLocaleDateString('pl-pl', {
                weekday: 'long',
                day: 'numeric',
                month: 'numeric'
            });
        })
        .catch(() => {
            console.log("Nie udało się pobrać danych na temat pogody");
        });
}

function load() {
    set_weather_today();

    const url = 'https://api.weatherbit.io/v2.0/forecast/daily?key=b729919c22744e40a686085b39ad7038&lang=pl&city=Bialystok&days=4';
    const forecastDiv = document.getElementById('forecast');

    fetch(url)
        .then(response => response.json())
        .then(data => {
            for (let i = 0; i < 4; i++) {
                const forecastTile = document.createElement('div');
                forecastTile.classList.add('forecast_tile');

                const icon = document.createElement('img');
                const temperature = document.createElement('div');
                const date = document.createElement('div');

                icon.classList.add('weather_icon');
                temperature.classList.add('weather_temperature');
                date.classList.add('weather_date');

                icon.setAttribute('src', get_icon_uri(data.data[i].weather.code));
                temperature.innerText = data.data[i].temp + "℃";

                const today = new Date();
                date.innerText = new Date(today.getFullYear(), today.getMonth(), today.getDate() + i + 1).toLocaleDateString('pl-pl', {
                    weekday: 'short',
                    day: 'numeric',
                    month: 'numeric'
                });

                forecastTile.appendChild(date);
                forecastTile.appendChild(icon);
                forecastTile.appendChild(temperature);

                forecastDiv.appendChild(forecastTile);
            }
        })
        .catch(() => {
            console.log("Nie udało się pobrać danych na temat pogody");
        });
}

load();