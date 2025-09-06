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

  // Obtener todas las entradas
  getEntradas(): Observable<InventarioItem[]> {
    return this.http.get<InventarioItem[]>(this.apiUrl);
  }

  // Obtener entradas por IdInventario
  getByEntrada(idInventario: number): Observable<InventarioItem[]> {
    return this.http.get<InventarioItem[]>(`${this.apiUrl}/by-inventario/${idInventario}`);
  }

  // Registrar nueva entrada
  addEntrada(item: Partial<InventarioItem>): Observable<any> {
    return this.http.post<any>(this.apiUrl, item);
  }

  // Actualizar entrada
  updateEntrada(id: number, item: InventarioItem): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, item);
  }

  // Eliminar entrada
  deleteEntrada(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }
}