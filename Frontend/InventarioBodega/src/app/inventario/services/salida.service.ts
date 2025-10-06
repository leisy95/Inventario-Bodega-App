import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SalidaService {
  private apiUrl = 'http://localhost:5244/api/salidas';

  constructor(private http: HttpClient) { }

  // Crear nueva salida en estado EN_PROCESO
  crearSalida(): Observable<any> {
    return this.http.post(`${this.apiUrl}/crear`, {});
  }

  // Agregar item a la salida en proceso
  agregarItem(idSalida: number, idItem: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/${idSalida}/agregar-item/${idItem}`, {});
  }

  // Quitar item de la salida
  quitarItem(idSalida: number, idItem: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${idSalida}/quitar-item/${idItem}`);
  }

  // Obtener la salida en proceso (por usuario)
  obtenerSalidaActual(): Observable<any> {
    return this.http.get(`${this.apiUrl}/actual`);
  }

  // Confirmar salida + cliente
  confirmarSalida(idSalida: number, clienteId: number) {
    // Enviar como objeto JSON que coincida con ConfirmarSalidaRequest
    return this.http.post(`/api/salidas/${idSalida}/confirmar`, { ClienteId: clienteId });
  }

  // Cancelar salida
  cancelarSalida(idSalida: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/${idSalida}/cancelar`, {});
  }
}