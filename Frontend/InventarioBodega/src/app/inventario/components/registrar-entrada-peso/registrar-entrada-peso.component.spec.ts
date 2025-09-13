import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MostrarVistaComponent } from './registrar-entrada-peso.component';

describe('MostrarVistaComponent', () => {
  let component: MostrarVistaComponent;
  let fixture: ComponentFixture<MostrarVistaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MostrarVistaComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MostrarVistaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
