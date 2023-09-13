export const formatDateTimeShort = (
  s?: string | null | undefined,
  style?: {
    dateStyle?: "short" | "full" | "long" | "medium" | undefined;
    timeStyle?: "short" | "full" | "long" | "medium" | undefined;
  }
) => {
  try {
    return Intl.DateTimeFormat("en-US", {
      dateStyle: style?.dateStyle ?? "short",
      timeStyle: style?.timeStyle ?? "short",
    }).format(new Date(s ?? ""));
  } catch (e) {
    return "Invalid date";
  }
};

export const makeDateString = (d: Date) =>
  `${d.getFullYear()}-${
    d.getMonth() + 1 < 10 ? `0${d.getMonth() + 1}` : d.getMonth() + 1
  }-${
    new Date().getDate() < 10
      ? `0${new Date().getDate()}`
      : `${new Date().getDate()}`
  }`;

export type LaundryRecordStatus =
  | "ONGOING"
  | "DONE"
  | "PROCESSING"
  | "PICKED_UP"
  | "DISCARDED";
export const laundryRecordStatuses: LaundryRecordStatus[] = [
  "PROCESSING",
  "PICKED_UP",
  "DONE",
  "DISCARDED",
];