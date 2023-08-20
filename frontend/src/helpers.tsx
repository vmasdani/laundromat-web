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
