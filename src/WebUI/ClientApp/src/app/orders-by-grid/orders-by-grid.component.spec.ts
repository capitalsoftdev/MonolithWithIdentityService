import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrdersByGridComponent } from './orders-by-grid.component';

describe('OrdersByGridComponent', () => {
  let component: OrdersByGridComponent;
  let fixture: ComponentFixture<OrdersByGridComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OrdersByGridComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OrdersByGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
