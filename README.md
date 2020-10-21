all commands on solution file level

1. Build App
```console
docker build -t geo-web --target final -f Dockerfile .
```

2. Launch WebApp
```console
docker run -p "9215:80" --env "GoogleTimeZone:ApiKey={PLACE GOOGLE API KEY}" --env "OpenWeatherMap:ApiKey={PLACE OPEN WEATHER API KEY}" geo-web
```

3. open browser
http://localhost:9215
