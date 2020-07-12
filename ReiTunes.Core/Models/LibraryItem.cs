﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using ReiTunes.Core;

namespace ReiTunes.Core {

    public class LibraryItem : Aggregate, IEquatable<LibraryItem> {
        private string _name;

        public string Name {
            get => _name;
            set {
                ApplyUncommitted(new LibraryItemNameChangedEvent(Guid.NewGuid(), AggregateId, DateTime.UtcNow, value));
                NotifyPropertyChanged();
            }
        }

        private string _filePath;

        public string FilePath {
            get => _filePath;
            set {
                ApplyUncommitted(new LibraryItemFilePathChangedEvent(Guid.NewGuid(), AggregateId, DateTime.UtcNow, value));
                NotifyPropertyChanged();
            }
        }

        private string _artist;

        public string Artist {
            get => _artist;
            set {
                ApplyUncommitted(new LibraryItemArtistChangedEvent(Guid.NewGuid(), AggregateId, DateTime.UtcNow, value));
                NotifyPropertyChanged();
            }
        }

        private string _album;

        public string Album {
            get => _album;
            set {
                ApplyUncommitted(new LibraryItemAlbumChangedEvent(Guid.NewGuid(), AggregateId, DateTime.UtcNow, value));
                NotifyPropertyChanged();
            }
        }

        private int _playCount = 0;

        public int PlayCount {
            get => _playCount;
        }

        //public string Text {
        //    get => _text;
        //    set {
        //        ApplyUncommitted(new SimpleTextAggregateUpdatedEvent(Guid.NewGuid(), Id, DateTime.UtcNow, value));
        //        NotifyPropertyChanged();
        //    }
        //}

        public LibraryItem() {
        }

        public LibraryItem(string relativePath) {
            var fileName = GetFileNameFromPath(relativePath);
            ApplyUncommitted(new LibraryItemCreatedEvent(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, fileName, relativePath));
        }

        public void IncrementPlayCount() {
            ApplyUncommitted(new LibraryItemPlayedEvent(Guid.NewGuid(), AggregateId, DateTime.UtcNow));
        }

        private string GetFileNameFromPath(string path) => path.Split('/').Last();

        protected override void RegisterAppliers() {
            this.RegisterApplier<LibraryItemCreatedEvent>(this.Apply);
            this.RegisterApplier<LibraryItemPlayedEvent>(this.Apply);
            this.RegisterApplier<LibraryItemNameChangedEvent>(this.Apply);
            this.RegisterApplier<LibraryItemFilePathChangedEvent>(this.Apply);
            this.RegisterApplier<LibraryItemAlbumChangedEvent>(this.Apply);
            this.RegisterApplier<LibraryItemArtistChangedEvent>(this.Apply);
        }

        private void Apply(LibraryItemCreatedEvent @event) {
            AggregateId = @event.AggregateId;
            _name = @event.Name;
            _filePath = @event.FilePath;
        }

        private void Apply(LibraryItemPlayedEvent @event) {
            _playCount++;
            NotifyPropertyChanged(nameof(PlayCount));
        }

        private void Apply(LibraryItemNameChangedEvent @event) {
            _name = @event.NewName;
            NotifyPropertyChanged(nameof(Name));
        }

        private void Apply(LibraryItemFilePathChangedEvent @event) {
            _filePath = @event.NewFilePath;
            NotifyPropertyChanged(nameof(FilePath));
        }

        private void Apply(LibraryItemAlbumChangedEvent @event) {
            _album = @event.NewAlbum;
            NotifyPropertyChanged(nameof(Album));
        }

        private void Apply(LibraryItemArtistChangedEvent @event) {
            _artist = @event.NewArtist;
            NotifyPropertyChanged(nameof(Artist));
        }

        public override bool Equals(object obj) {
            return this.Equals(obj as LibraryItem);
        }

        public bool Equals(LibraryItem other) {
            if (Object.ReferenceEquals(other, null))
                return false;

            if (Object.ReferenceEquals(this, other))
                return true;

            return this.AggregateId == other.AggregateId &&
                this.CreatedTimeUtc == other.CreatedTimeUtc &&
                this.Name == other.Name &&
                this.FilePath == other.FilePath &&
                this.Artist == other.Artist &&
                this.Album == other.Album &&
                this.PlayCount == other.PlayCount;
        }

        public static bool operator ==(LibraryItem lhs, LibraryItem rhs) {
            // Check for null on left side.
            if (Object.ReferenceEquals(lhs, null)) {
                if (Object.ReferenceEquals(rhs, null)) {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }

        public static bool operator !=(LibraryItem lhs, LibraryItem rhs) {
            return !(lhs == rhs);
        }

        public override int GetHashCode() {
            return AggregateId.GetHashCode();
        }
    }
}