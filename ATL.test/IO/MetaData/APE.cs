﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ATL.AudioData;
using System.Collections.Generic;
using System.IO;
using Commons;

namespace ATL.test.IO.MetaData
{
    [TestClass]
    public class APE : MetaIOTest
    {
        public APE()
        {
            emptyFile = "MP3/empty.mp3";
            notEmptyFile = "MP3/APE.mp3";
            tagType = MetaDataIOFactory.TAG_APE;

            // Initialize specific test data (Publisher and Description fields not supported in APE tag)
            testData.Publisher = null;
            testData.GeneralDescription = null;

            // Initialize specific test data (Picture native codes are strings)
            testData.Pictures.Clear();
            PictureInfo pic = new PictureInfo(Commons.ImageFormat.Jpeg, MetaDataIOFactory.TAG_ANY, "COVER ART (FRONT)");
            byte[] data = System.IO.File.ReadAllBytes(TestUtils.GetResourceLocationRoot() + "_Images/pic1.jpeg");
            pic.PictureData = data;
            pic.ComputePicHash();
            testData.Pictures.Add(pic);

            pic = new PictureInfo(Commons.ImageFormat.Png, MetaDataIOFactory.TAG_ANY, "COVER ART (BACK)");
            data = System.IO.File.ReadAllBytes(TestUtils.GetResourceLocationRoot() + "_Images/pic1.png");
            pic.PictureData = data;
            pic.ComputePicHash();
            testData.Pictures.Add(pic);
        }


        [TestMethod]
        public void TagIO_R_APE() // My deepest apologies for this dubious method name
        {
            String location = TestUtils.GetResourceLocationRoot() + notEmptyFile;
            AudioDataManager theFile = new AudioDataManager( AudioData.AudioDataIOFactory.GetInstance().GetFromPath(location) );

            readExistingTagsOnFile(theFile);
        }

        [TestMethod]
        public void TagIO_RW_APE_Empty()
        {
            test_RW_Empty(emptyFile, true, true, true);
        }

        [TestMethod]
        public void TagIO_RW_APE_Existing()
        {
            // Hash check NOT POSSIBLE YET mainly due to tag order differences
            test_RW_Existing(notEmptyFile, 2, true, true, false);
        }

        [TestMethod]
        public void TagIO_RW_APE_Unsupported_Empty()
        {
            test_RW_Unsupported_Empty(emptyFile);
        }

        private void checkTrackDiscZeroes(FileStream fs)
        {
            using (BinaryReader r = new BinaryReader(fs))
            {
                byte[] bytes = new byte[20];
                fs.Seek(0, SeekOrigin.Begin);
                Assert.IsTrue(StreamUtils.FindSequence(fs, Utils.Latin1Encoding.GetBytes("DISCNUMBER")));
                fs.Seek(1, SeekOrigin.Current);
                String s = StreamUtils.ReadNullTerminatedString(r, System.Text.Encoding.ASCII);
                Assert.AreEqual("03/04", s.Substring(0, s.Length-1));

                fs.Seek(0, SeekOrigin.Begin);
                Assert.IsTrue(StreamUtils.FindSequence(fs, Utils.Latin1Encoding.GetBytes("TRACK")));
                fs.Seek(1, SeekOrigin.Current);
                s = StreamUtils.ReadNullTerminatedString(r, System.Text.Encoding.ASCII);
                Assert.AreEqual("06/06", s.Substring(0, s.Length - 1));
            }
        }

        [TestMethod]
        public void TagIO_RW_APE_UpdateKeepTrackDiscZeroes()
        {
            StreamDelegate dlg = new StreamDelegate(checkTrackDiscZeroes);
            test_RW_UpdateTrackDiscZeroes(notEmptyFile, false, false, dlg);
        }

        [TestMethod]
        public void TagIO_RW_APE_UpdateFormatTrackDiscZeroes()
        {
            StreamDelegate dlg = new StreamDelegate(checkTrackDiscZeroes);
            test_RW_UpdateTrackDiscZeroes(notEmptyFile, true, true, dlg);
        }

        [TestMethod]
        public void TagIO_R_APE_Rating()
        {
            assumeRatingInFile("_Ratings/mediaMonkey_4.1.19.1859/0.ape", 0, MetaDataIOFactory.TAG_APE);
            assumeRatingInFile("_Ratings/mediaMonkey_4.1.19.1859/0.5.ape", 0.5 / 5, MetaDataIOFactory.TAG_APE);
            assumeRatingInFile("_Ratings/mediaMonkey_4.1.19.1859/1.ape", 1.0 / 5, MetaDataIOFactory.TAG_APE);
            assumeRatingInFile("_Ratings/mediaMonkey_4.1.19.1859/1.5.ape", 1.5 / 5, MetaDataIOFactory.TAG_APE);
            assumeRatingInFile("_Ratings/mediaMonkey_4.1.19.1859/2.ape", 2.0 / 5, MetaDataIOFactory.TAG_APE);
            assumeRatingInFile("_Ratings/mediaMonkey_4.1.19.1859/2.5.ape", 2.5 / 5, MetaDataIOFactory.TAG_APE);
            assumeRatingInFile("_Ratings/mediaMonkey_4.1.19.1859/3.ape", 3.0 / 5, MetaDataIOFactory.TAG_APE);
            assumeRatingInFile("_Ratings/mediaMonkey_4.1.19.1859/3.5.ape", 3.5 / 5, MetaDataIOFactory.TAG_APE);
            assumeRatingInFile("_Ratings/mediaMonkey_4.1.19.1859/4.ape", 4.0 / 5, MetaDataIOFactory.TAG_APE);
            assumeRatingInFile("_Ratings/mediaMonkey_4.1.19.1859/4.5.ape", 4.5 / 5, MetaDataIOFactory.TAG_APE);
            assumeRatingInFile("_Ratings/mediaMonkey_4.1.19.1859/5.ape", 1, MetaDataIOFactory.TAG_APE);

            assumeRatingInFile("_Ratings/musicBee_3.1.6512/0.ape", 0, MetaDataIOFactory.TAG_APE);
            assumeRatingInFile("_Ratings/musicBee_3.1.6512/0.5.ape", 0.5 / 5, MetaDataIOFactory.TAG_APE);
            assumeRatingInFile("_Ratings/musicBee_3.1.6512/1.ape", 1.0 / 5, MetaDataIOFactory.TAG_APE);
            assumeRatingInFile("_Ratings/musicBee_3.1.6512/1.5.ape", 1.5 / 5, MetaDataIOFactory.TAG_APE);
            assumeRatingInFile("_Ratings/musicBee_3.1.6512/2.ape", 2.0 / 5, MetaDataIOFactory.TAG_APE);
            assumeRatingInFile("_Ratings/musicBee_3.1.6512/2.5.ape", 2.5 / 5, MetaDataIOFactory.TAG_APE);
            assumeRatingInFile("_Ratings/musicBee_3.1.6512/3.ape", 3.0 / 5, MetaDataIOFactory.TAG_APE);
            assumeRatingInFile("_Ratings/musicBee_3.1.6512/3.5.ape", 3.5 / 5, MetaDataIOFactory.TAG_APE);
            assumeRatingInFile("_Ratings/musicBee_3.1.6512/4.ape", 4.0 / 5, MetaDataIOFactory.TAG_APE);
            assumeRatingInFile("_Ratings/musicBee_3.1.6512/4.5.ape", 4.5 / 5, MetaDataIOFactory.TAG_APE);
            assumeRatingInFile("_Ratings/musicBee_3.1.6512/5.ape", 1, MetaDataIOFactory.TAG_APE);
        }

        [TestMethod]
        public void TagIO_RW_APE_ID3v1()
        {
            test_RW_Cohabitation(MetaDataIOFactory.TAG_APE, MetaDataIOFactory.TAG_ID3V1);
        }

        [TestMethod]
        public void TagIO_RW_APE_ID3v2()
        {
            test_RW_Cohabitation(MetaDataIOFactory.TAG_APE, MetaDataIOFactory.TAG_ID3V2);
        }
    }
}
