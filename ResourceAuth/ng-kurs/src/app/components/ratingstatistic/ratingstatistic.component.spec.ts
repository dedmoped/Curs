import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RatingstatisticComponent } from './ratingstatistic.component';

describe('RatingstatisticComponent', () => {
  let component: RatingstatisticComponent;
  let fixture: ComponentFixture<RatingstatisticComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RatingstatisticComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RatingstatisticComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
