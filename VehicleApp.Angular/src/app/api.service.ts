import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { VehicleView } from './shared.models';
import { Manufacturer, Vehicle } from './vehicle-edit/vehicle-edit.component';
import { Category } from './categories-edit/categories-edit.component';



@Injectable({
  providedIn: 'root',
})
export class ApiService {
  constructor(private http: HttpClient) { }

  getVehicles(): Observable<VehicleView[]> {
    return this.http.get<VehicleView[]>("/vehicles");
  }

  getVehicle(id: number): Observable<Vehicle> {
    const url = `/vehicles/${id}`;
    return this.http.get<Vehicle>(url);
  }

  editVehicle(vehicle: Vehicle) {
    const url = `/vehicles/${vehicle.id}`;
    return this.http.put<Vehicle>(url, vehicle);
  }

  addVehicle(vehicle: Vehicle) {
    const url = `/vehicles`;
    return this.http.post<Vehicle>(url, vehicle);
  }

  deleteVehicle(vehicle: Vehicle) {
    const url = `/vehicles/${vehicle.id}`;
    return this.http.delete<Vehicle>(url);
  }

  getManufacturers(): Observable<Manufacturer[]> {
    return this.http.get<Manufacturer[]>("/manufacturers");
  }

  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>("/categories");
  }

  saveCategories(categories: Category[]): Observable<Category[]> {
    return this.http.put<Category[]>("/categories", categories);
  }
}
