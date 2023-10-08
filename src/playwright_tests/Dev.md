
```sh
npx playwright install
```

[playwright.dev](https://playwright.dev/docs/test-api-testing)  
[github.com/microsoft/playwright/](https://github.com/microsoft/playwright/blob/main/examples/github-api/tests/test-api.spec.ts)  

```sh
npx playwright show-report
npx playwright test --reporter=list ApiPort.spec.ts
npx playwright show-report
npx playwright test --debug
npx playwright test --reporter=list

npm install dotenv --save
npm install ajv --save
```
---
```sh
# Работа с перемінними окружения
unset MY_```BASE_URL
env | grep MY_
echo $MY_BASE_URL

export MY_BASE_URL=http://localhost:5100
export MY_ASPNETCORE_ENVIRONMENT=Development

# или
export $(grep -v '^#' .env | xargs) && env | grep MY_

echo $MY_BASE_URL
echo $MY_ASPNETCORE_ENVIRONMENT
```

```sh
# Запуск
npx playwright test tests/PizzaWeb_Api.spec.ts
```
