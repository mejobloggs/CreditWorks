import { Component } from '@angular/core';
import { ApiService } from '../api.service';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-vehicle-edit',
  templateUrl: './vehicle-edit.component.html',
  styleUrls: ['./vehicle-edit.component.css']
})

export class VehicleEditComponent {
  vehicle: Vehicle = {
    ownerName: '',
    manufactureYear: 0,
    weightInKg: 0,
    manufacturerId: 0
  };
  manufacturers: Manufacturer[] = [];
  constructor(
    private location: Location,
    private route: ActivatedRoute,
    private apiService: ApiService) { }

  ngOnInit(): void {
    const vehicleId = Number(this.route.snapshot.paramMap.get('id'));
    if (vehicleId > 0) {
      this.apiService.getVehicle(vehicleId).subscribe(v => { this.vehicle = v });
    }
    this.apiService.getManufacturers().subscribe(m => { this.manufacturers = m });
  }

  save(): void {
    if (this.vehicle) {
      if (this.vehicle.id && this.vehicle.id > 0) {
        this.apiService.editVehicle(this.vehicle).subscribe(() => { this.goBack(); });
      }
      else {
        this.apiService.addVehicle(this.vehicle).subscribe(() => { this.goBack(); });
      }
    }
  }

  delete(): void {
    this.apiService.deleteVehicle(this.vehicle).subscribe(() => { this.goBack(); })
  }

  goBack(): void {
    this.location.back();
  }
}

export interface Vehicle {
  id?: number;
  ownerName: string;
  manufactureYear: number;
  weightInKg: number;
  manufacturerId: number;
}

export interface Manufacturer {
  id: number,
  name: string
}
