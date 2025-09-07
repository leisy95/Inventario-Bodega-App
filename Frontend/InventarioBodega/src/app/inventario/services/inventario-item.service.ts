import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { InventarioItem } from '../models/inventario-item.model';

@Injectable({
  providedIn: 'root'
})
export class InventarioItemService {
  private apiUrl = 'http://localhost:5244/api/inventarioitem';

  constructor(private http: HttpClient) { }

  // Obtener todas las entradas inventarioItem
  getEntradas(): Observable<InventarioItem[]> {
    return this.http.get<InventarioItem[]>(`${this.apiUrl}/all-items`);
  }

  // Obtener entradas por IdInventario
  getByEntrada(idInventario: number): Observable<InventarioItem[]> {
    return this.http.get<InventarioItem[]>(`${this.apiUrl}/by-inventario/${idInventario}`);
  }

  // Buscar entradas por referenciaPeso
  getByReferencia(referencia: string) {
    return this.http.get<InventarioItem[]>(`${this.apiUrl}/by-referencia/${referencia}`);
  }

  // Registrar nueva entrada
  addEntrada(item: Partial<InventarioItem>): Observable<any> {
    return this.http.post<any>(this.apiUrl, item);
  }

  // Actualizar entrada solo peso
  updatePesoEntrada(id: number, pesoActual: number) {
    return this.http.put<any>(`${this.apiUrl}/${id}`, { pesoActual });
  }

  // Eliminar entrada de inventarioIten
  deleteEntrada(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }
}