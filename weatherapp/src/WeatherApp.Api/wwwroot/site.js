const countrySelect = document.getElementById("country");
const citySelect = document.getElementById("city");
const message = document.getElementById("message");
const result = document.getElementById("result");

function setMessage(text, isError) {
    message.textContent = text;
    message.className = isError ? "error" : "";
}

async function getJson(url) {
    const response = await fetch(url);

    if (!response.ok) {
        const body = await response.json().catch(() => null);
        throw new Error(body && body.message ? body.message : "Request failed.");
    }

    return response.json();
}

async function loadCountries() {
    try {
        const countries = await getJson("/api/countries");

        countrySelect.innerHTML = '<option value="">Select a country</option>';

        countries.forEach(c => {
            const option = document.createElement("option");
            option.value = c.code;
            option.textContent = c.name;
            countrySelect.appendChild(option);
        });

        setMessage("");
    } catch (e) {
        setMessage("Failed load countries. " + e.message, true);
    }
}

async function loadCities(countryCode) {
    result.classList.add("hidden");

    if (!countryCode) {
        citySelect.disabled = true;
        citySelect.innerHTML = '<option value="">Select a country first</option>';
        setMessage("");
        return;
    }

    try {
        const cities = await getJson("/api/countries/" + countryCode + "/cities");

        citySelect.innerHTML = '<option value="">Select a city</option>';

        cities.forEach(c => {
            const option = document.createElement("option");
            option.value = c.name;
            option.textContent = c.name;
            citySelect.appendChild(option);
        });

        citySelect.disabled = false;
        setMessage("");
    } catch (e) {
        setMessage("Could not load cities. " + e.message, true);
    }
}

async function loadWeather(cityName) {
    result.classList.add("hidden");

    if (!cityName) {
        setMessage("");
        return;
    }

    setMessage("Loading...");

    try {
        const w = await getJson("/api/weather/" + encodeURIComponent(cityName));

        document.getElementById("location").textContent = w.location;
        document.getElementById("time").textContent = new Date(w.timeUtc).toISOString().replace("T", " ").substring(0, 16);
        document.getElementById("wind").textContent = w.windSpeedMph + " mph from " + w.windDirection;
        document.getElementById("visibility").textContent = w.visibilityMiles + " miles";
        document.getElementById("sky").textContent = w.skyConditions;
        document.getElementById("temperature").textContent = w.temperatureF + " \u00B0F / " + w.temperatureC + " \u00B0C";
        document.getElementById("dewpoint").textContent = w.dewPointF + " \u00B0F / " + w.dewPointC + " \u00B0C";
        document.getElementById("humidity").textContent = w.relativeHumidity + " %";
        document.getElementById("pressure").textContent = w.pressureHpa + " hPa";

        result.classList.remove("hidden");
        setMessage("");
    } catch (e) {
        setMessage("Failed load weather. " + e.message, true);
    }
}

countrySelect.addEventListener("change", () => loadCities(countrySelect.value));
citySelect.addEventListener("change", () => loadWeather(citySelect.value));

loadCountries();
