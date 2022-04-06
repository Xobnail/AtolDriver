using Atol.Drivers10.Fptr;
using System;
using Newtonsoft.Json;
using AtolDriver.models;
using System.Collections.Generic;

namespace AtolDriver
{
    public class Interface
    {
        IFptr fptr;
        Operator cashier;
        Receipt receipt;

        public Interface(int port, int speed)
        {
            fptr = new Fptr();
            fptr.setSingleSetting(Constants.LIBFPTR_SETTING_MODEL, Constants.LIBFPTR_MODEL_ATOL_AUTO.ToString());
            fptr.setSingleSetting(Constants.LIBFPTR_SETTING_PORT, Constants.LIBFPTR_PORT_COM.ToString());
            fptr.setSingleSetting(Constants.LIBFPTR_SETTING_COM_FILE, "COM" + port);
            fptr.setSingleSetting(Constants.LIBFPTR_SETTING_BAUDRATE, speed.ToString());
            fptr.applySingleSettings();
        }

        public void SendJson<T>(T request)
        {
            string task = JsonConvert.SerializeObject(request);
            fptr.setParam(Constants.LIBFPTR_PARAM_JSON_DATA, task);
            fptr.processJson();
        }

        public void OpenConnection()
        {
            fptr.open();
        }

        public void SetOperator(string operatorName, string operatorInn)
        {
            cashier = new Operator
            {
                Name = operatorName,
                Vatin = operatorInn
            };
        }

        public void OpenShift()
        {
            SendJson(new OpenShift
            {
                Operator = cashier
            });
        }

        public void OpenReceipt()
        {
            receipt = new Receipt
            {
                Type = "sell",
                TaxationType = "osn",
                Operator = cashier,
                Items = new List<Item>(),
                Payments = new List<Payments>()
            };
        }

        public void AddPosition(string name, double price, double quantity, string measurementUnit, string paymentObject)
        {
            receipt.Items.Add(new Item
            {
                Type = "position",
                Name = name,
                Price = price,
                Quantity = quantity,
                MeasurementUnit = measurementUnit,
                PaymentObject = paymentObject,
                Amount = price * quantity,
                Tax = new Tax { Type = "vat20" }
            });
        }

        public void Pay(string type, double sum)
        {
            receipt.Payments.Add(new Payments
            {
                Type = type,
                Sum = sum
            });
        }

        public void CloseReceipt()
        {
            SendJson(receipt);
        }

        public void CloseShift()
        {
            SendJson(new CloseShift
            {
                Type = "closeShift",
                Operator = cashier
            });
        }

        public void PrintStatus()
        {
            fptr.setParam(Constants.LIBFPTR_PARAM_DATA_TYPE, Constants.LIBFPTR_DT_STATUS);
            fptr.queryData();

            Console.WriteLine("---------Status-----------");
            Console.WriteLine(" operatorID = " + fptr.getParamInt(Constants.LIBFPTR_PARAM_OPERATOR_ID));
            Console.WriteLine(" logicalNumber = " + fptr.getParamInt(Constants.LIBFPTR_PARAM_LOGICAL_NUMBER));
            Console.WriteLine(" shiftState = " + fptr.getParamInt(Constants.LIBFPTR_PARAM_SHIFT_STATE));
            Console.WriteLine(" model = " + fptr.getParamInt(Constants.LIBFPTR_PARAM_MODEL));
            Console.WriteLine(" mode = " + fptr.getParamInt(Constants.LIBFPTR_PARAM_MODE));
            Console.WriteLine(" submode = " + fptr.getParamInt(Constants.LIBFPTR_PARAM_SUBMODE));
            Console.WriteLine(" receiptNumber = " + fptr.getParamInt(Constants.LIBFPTR_PARAM_RECEIPT_NUMBER));
            Console.WriteLine(" documentNumber = " + fptr.getParamInt(Constants.LIBFPTR_PARAM_DOCUMENT_NUMBER));
            Console.WriteLine(" shiftNumber = " + fptr.getParamInt(Constants.LIBFPTR_PARAM_SHIFT_NUMBER));
            Console.WriteLine(" receiptType = " + fptr.getParamInt(Constants.LIBFPTR_PARAM_RECEIPT_TYPE));
            Console.WriteLine(" documentType = " + fptr.getParamInt(Constants.LIBFPTR_PARAM_DOCUMENT_TYPE));
            Console.WriteLine(" lineLength = " + fptr.getParamInt(Constants.LIBFPTR_PARAM_RECEIPT_LINE_LENGTH));
            Console.WriteLine(" lineLengthPix = " + fptr.getParamInt(Constants.LIBFPTR_PARAM_RECEIPT_LINE_LENGTH_PIX));
            Console.WriteLine(" receiptSum = " + fptr.getParamDouble(Constants.LIBFPTR_PARAM_RECEIPT_SUM));
            Console.WriteLine(" isFiscalDevice = " + fptr.getParamBool(Constants.LIBFPTR_PARAM_FISCAL));
            Console.WriteLine(" isFiscalFN = " + fptr.getParamBool(Constants.LIBFPTR_PARAM_FN_FISCAL));
            Console.WriteLine(" isFNPresent = " + fptr.getParamBool(Constants.LIBFPTR_PARAM_FN_PRESENT));
            Console.WriteLine(" isInvalidFN = " + fptr.getParamBool(Constants.LIBFPTR_PARAM_INVALID_FN));
            Console.WriteLine(" isCashDrawerOpened = " + fptr.getParamBool(Constants.LIBFPTR_PARAM_CASHDRAWER_OPENED));
            Console.WriteLine(" isPaperPresent = " + fptr.getParamBool(Constants.LIBFPTR_PARAM_RECEIPT_PAPER_PRESENT));
            Console.WriteLine(" isPaperNearEnd = " + fptr.getParamBool(Constants.LIBFPTR_PARAM_PAPER_NEAR_END));
            Console.WriteLine(" isCoverOpened = " + fptr.getParamBool(Constants.LIBFPTR_PARAM_COVER_OPENED));
            Console.WriteLine(" isPrinterConnectionLost = " + fptr.getParamBool(Constants.LIBFPTR_PARAM_PRINTER_CONNECTION_LOST));
            Console.WriteLine(" isPrinterError = " + fptr.getParamBool(Constants.LIBFPTR_PARAM_PRINTER_ERROR));
            Console.WriteLine(" isCutError = " + fptr.getParamBool(Constants.LIBFPTR_PARAM_CUT_ERROR));
            Console.WriteLine(" isPrinterOverheat = " + fptr.getParamBool(Constants.LIBFPTR_PARAM_PRINTER_OVERHEAT));
            Console.WriteLine(" isDeviceBlocked = " + fptr.getParamBool(Constants.LIBFPTR_PARAM_BLOCKED));
            Console.WriteLine(" dateTime = " + fptr.getParamDateTime(Constants.LIBFPTR_PARAM_DATE_TIME));
            Console.WriteLine(" serialNumber = " + fptr.getParamString(Constants.LIBFPTR_PARAM_SERIAL_NUMBER));
            Console.WriteLine(" modelName = " + fptr.getParamString(Constants.LIBFPTR_PARAM_MODEL_NAME));
            Console.WriteLine(" firmwareVersion = " + fptr.getParamString(Constants.LIBFPTR_PARAM_UNIT_VERSION));
            Console.WriteLine("--------------------------");
        }
    }
}
