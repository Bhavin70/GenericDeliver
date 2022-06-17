using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.StaticStuff
{
    public class NotificationConstants
    {

        #region Notifications
        public enum NotificationsE
        {
            NEW_RATE_AND_QUOTA_DECLARED = 1,
            BOOKING_APPROVAL_REQUIRED = 2,
            BOOKING_APPROVED_BY_DIRECTORS = 3,
            BOOKING_CONFIRMED = 4,
            BOOKING_REJECTED_BY_DIRECTORS = 5,
            BOOKINGS_CLOSED = 6,
            TODAYS_STOCK_CONFIRMED = 7,
            TODAYS_STOCK_AS_PER_ACCOUNTANT = 8,
            BOOKING_ACCEPTED_BY_CNF = 9,
            BOOKING_REJECTED_BY_CNF = 10,
            LOADING_SLIP_CONFIRMATION_REQUIRED = 11,
            LOADING_SLIP_CONFIRMED = 12,
            LOADING_SLIP_CANCELLED = 13,
            VEHICLE_OUT_FOR_DELIVERY = 14,
            LOADING_QUOTA_DECLARED = 15,
            LOADING_STOPPED = 16,
            STRAIGHT_TO_BEND_TRANSFER_REQUEST = 17,
            INVOICE_APPROVAL_REQUIRED = 18,
            INVOICE_APPROVED_BY_DIRECTOR = 19,
            INVOICE_REJECTED_BY_DIRECTOR = 20,
            INVOICE_ACCEPTED_BY_DISTRIBUTOR = 21,
            INVOICE_REJECTED_BY_DISTRIBUTOR = 22,
            INVOICE_ACCEPTANCE_REQUIRED=23,
            NEW_TRANSPORT_SLIP_GENERATE = 24,
            VEHICLE_IN_FOR_DELIVERY = 25,
            BOOKING_HOLD_BY_ADMIN_OR_DIRECTOR=26,                        //Priyanka [30-07-2018],
            New_Booking = 27,                                            //Priyanka [30-07-2018]
            DIRECTOR_REMARK_IN_BOOKING = 28,                              //Priyanka [03-10-2018]

            //Priyanka [09-10-2018] : Added to set status about vehicles.
            LOADING_GATE_IN = 29,
            VEHICLE_REPORTED_FOR_LOADING = 30,
            LOADING_VEHICLE_CLEARANCE_TO_SEND_IN = 31,
            SUPERWISOR_ALLOCATION_FOR_VEHICLE=32,

            SIZE_CHANGES_IN_BOOKING = 33, //AmolG[2020-Feb-11]

            BOOKING_DUE = 34, 
            LOADING_VEHICLE_OUT = 58,   ///Reshma Added For Vehicle Out as per Parmeshwar requirement.
        }

        #endregion

        public enum NotificationTypeE
        {
            ALERT = 1,
            EMAIL = 2,
            SMS = 3
        }
    }
}
