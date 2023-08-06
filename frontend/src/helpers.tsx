export const formatDateTimeShort = (s?: string | null | undefined) => {
  try {
    return Intl.DateTimeFormat("en-US", {
      dateStyle: "short",
      timeStyle: "short",
    }).format(new Date(s ?? ""));
  } catch (e) {
    return "Invalid date";
  }
};

export type LaundryRecordStatus = "ONGOING" | "DONE";
export const laundryRecordStatuses:LaundryRecordStatus[] = ["ONGOING" , "DONE"];

