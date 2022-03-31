import http from 'k6/http';
import { sleep } from 'k6';

export default function () {
  http.get('https://cloudxapi.azurewebsites.net/api/catalog-brands');
  sleep(1);
}