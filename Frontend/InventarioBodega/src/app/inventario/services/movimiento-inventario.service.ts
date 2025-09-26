import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Inventario {
  id: number;
  nombre: string;
}

export interface InventarioItem {
  id: number;
  codigo: string;
}

export interface MovimientoInventario {
  id: number;
  referencia: string;
  referenciaPeso: string;
  peso: number;
  fecha: string;
  tipo: string;
  usuario: string;
  Inventario: { Id: number; Nombre: string };
  InventarioItem: { Id: number; Codigo: string };
}

@Injectable({
  providedIn: 'root'
})
export class MovimientoInventarioService {
  private apiUrl = 'http://localhost:5244/api/MovimientosInventario';

  constructor(private http: HttpClient) { }

  getMovimientos(): Observable<MovimientoInventario[]> {
    return this.http.get<MovimientoInventario[]>(this.apiUrl);
  }

  getAuditoria(fechaInicio?: string, fechaFin?: string) {
  let params: any = {};
  if (fechaInicio) params.fechaInicio = fechaInicio;
  if (fechaFin) params.fechaFin = fechaFin;

  return this.http.get<any>(`${this.apiUrl}/movimientosinventario/auditoria`, { params });
}

  getResumen() {
  return this.http.get<any>(`${this.apiUrl}/resumen`);
}
}