import { PlaywrightTestConfig, test, expect } from "@playwright/test";
import * as ajv from "ajv";

let _number: number;
let _sys_id: string;

// Послідовне виконання
test.describe.configure({ mode: "serial" });

// const BASE_URL = "https://localhost:5101";
const BASE_URL = process.env.MY_BASE_URL;
const MY_ENV = process.env.MY_ASPNETCORE_ENVIRONMENT;

const testName = "Test_____1";
const testPrice = 17;

// const config: PlaywrightTestConfig = {
//   use: {
//     baseUrl: process.env.MY_BASE_URL;
//   },
// };
test("basic test", async ({ page }) => {
  const response = await page.goto(`${BASE_URL}/version`);
  const responseJson = await response.json();
  expect(response.status()).toBe(200);
});

test("get_version", async ({ page }) => {
  const response = await page.goto(`${BASE_URL}/version`).then((res) => res);
  const responseJson = await response.json();
  expect(response.status()).toBe(200);

  expect(responseJson.ver_app).toBe("0.0.0.7");
  // expect(responseJson.env).toContain(MY_ENV);
  expect(responseJson.env).toEqual(MY_ENV);
  expect(responseJson.name).toContain("WebApp.Api");

  const validator = new ajv.default();
  const schema = {
    type: "object",
    properties: {
      name: { type: "string" },
      ver_app: { type: "string" },
      machineName: { type: "string" },
      correlation_id: { type: "string", minLength: 10 },
      ip_address: { type: "string", minLength: 5 },
      // ip_address: { type: "string", format: "ipv4" },
      env: { type: "string" },
      date: { type: "string" },
      datarows: { type: "integer", nullable: true },
    },
    required: ["name"],
  };

  //const isValid = validator.compile<responseJson>(schema);
  const isValid = validator.validate(schema, responseJson);
  expect(isValid).toBe(true);
});

// Get All
test("Get all pizzas", async ({ request, baseURL }) => {
  console.log(`${baseURL}/pizzas`);
  const _response = await request.get(`${baseURL}/pizzas`);
  const res = await _response.json();
  //console.log(res.slice(0, 3));
  expect(res.length).toBeGreaterThan(0);
  expect(_response.status()).toBe(200);
});
/*
// Create
https: test("Create an Pizza", async ({ request, baseURL }) => {
  //console.log(`${baseURL}/pizzas`);
  const _response = await request.post(`${baseURL}/pizzas`, {
    data: {
      name: testName,
      price: testPrice,
    },
    headers: {
      Accept: "application/json",
    },
  });
  //expect(_response.status()).toBe(201);
  expect(_response.ok()).toBeTruthy();
  const res = await _response.json();
  //console.log(res);
  //expect(responseJson.name).toContain("PizzaWeb");
  expect(res.name).toEqual(testName);
  expect(res.price).toBe(testPrice);
  expect(res.id).toBeGreaterThan(0);
  _sys_id = res.id;
  console.log(`Створили ${_sys_id}`);
});
*/

/*

// Get by ID
test("Get Pizza", async ({ request, baseURL }) => {
  //console.log(`${baseURL}/pizzas/${_sys_id}`);
  const _response = await request.get(`${baseURL}/pizzas/${_sys_id}`);
  const res = await _response.json();
  // console.log(res);
  expect(_response.status()).toBe(200);
  // expect(res.isSuccess).toBe(true);
  expect(res.data.id).toBeGreaterThan(0);
  expect(res).toMatchObject({ isSuccess: true, statusCode: 200 });
});

// Uppdate
test("Put(Modify) an Pizza", async ({ request, baseURL }) => {
  //console.log(`${baseURL}/pizzas/${_sys_id}`);
  const _response = await request.put(`${baseURL}/pizzas/${_sys_id}`, {
    data: {
      name: testName + "7",
      price: testPrice + 7,
    },
  });
  //const res = await _response.json();
  //console.log(res);
  expect(_response.status()).toBe(204);
  expect(_response.ok()).toBeTruthy();
  console.log(`Обновили ${_sys_id}`);
});

// Delete
test("Delete an Pizza", async ({ request, baseURL }) => {
  const _response = await request.delete(`${baseURL}/pizzas/${_sys_id}`);
  // console.log(await _response.json());
  expect(_response.status()).toBe(204);
  expect(_response.ok()).toBeTruthy();
  console.log(`Видалили ${_sys_id}`);
});

*/
