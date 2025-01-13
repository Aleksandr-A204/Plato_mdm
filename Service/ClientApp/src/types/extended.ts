export interface Option {
  display: string,
  value: string
}

export interface SelectEvent {
  originalEvent: Event,
  value: Option
}

export interface MenuItem {
  label: string;
  icon: string;
  modalType?: string;
}
